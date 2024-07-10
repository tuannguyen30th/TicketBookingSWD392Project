using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Settings;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.Repo.SeedData;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Utilities;
using SWD.TicketBooking.Repo.Helpers;
using Microsoft.AspNetCore.Mvc;
using static QRCoder.PayloadGenerator;
using Org.BouncyCastle.Ocsp;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SWD.TicketBooking.Service.Services;

public class IdentityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;
    private readonly IFirebaseService _firebaseService;
    private readonly IEmailService _emailService;
    private static readonly HttpClient httpClient = new HttpClient();
    private readonly IUserService _userService;

    public IdentityService(IUserService userService, IEmailService emailService, IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettingsOptions, IFirebaseService firebaseService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _emailService = emailService;
        _jwtSettings = jwtSettingsOptions.Value;
        _firebaseService = firebaseService;
    }
    public async Task<AccessTokenModel> CheckAccessToken(string accessToken)
    {
        try
        {
            SecurityToken newToken = null;
            var existingUser = new User();
            var tokenInfoUrl = $"https://www.googleapis.com/oauth2/v3/tokeninfo?access_token={accessToken}";
            var response = await httpClient.GetAsync(tokenInfoUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new AccessTokenModel { Success = false, ErrorMessage = errorContent.ToUpper() };
            }

            var tokenInfo = await response.Content.ReadAsStringAsync();
            var userInfoUrl = $"https://www.googleapis.com/oauth2/v1/userinfo?access_token={accessToken}";
            var userInfoResponse = await httpClient.GetAsync(userInfoUrl);

            if (!userInfoResponse.IsSuccessStatusCode)
            {
                var userInfoError = await userInfoResponse.Content.ReadAsStringAsync();
                return new AccessTokenModel { Success = false, ErrorMessage = userInfoError.ToUpper() };
            }

            var userInfo = await userInfoResponse.Content.ReadAsStringAsync();
            var user = JObject.Parse(userInfo);

            var userResultNew = new
            {
                Id = user["id"]?.ToString(),
                Email = user["email"]?.ToString(),
                Name = user["name"]?.ToString(),
                Picture = user["picture"]?.ToString()
            };

            existingUser = await _userService.GetUserByEmailToLoginGG(userResultNew.Email);
            var newUser = new User();
            var handler = new JwtSecurityTokenHandler();
            if (existingUser != null)
            {
                existingUser.TokenExpiration = DateTime.UtcNow.AddHours(1);
                _unitOfWork.UserRepository.Update(existingUser);
                newToken = CreateJwtToken(existingUser);
                return new AccessTokenModel { Success = true, Token = new JwtSecurityTokenHandler().WriteToken(newToken) };
            }
            else
            {
                newUser = new User
                {
                    UserID = Guid.NewGuid(),
                    Email = userResultNew.Email,
                    Avatar = userResultNew.Picture,
                    Balance = 0,
                    CreateDate = DateTime.Now,
                    Password = "",
                    FullName = userResultNew.Name,
                    IsVerified = true,
                    Status = SD.GeneralStatus.ACTIVE,
                    TokenExpiration = DateTime.UtcNow.AddHours(1),
                    RoleID = new Guid("E6E2FCD6-22F0-426B-A3A0-DD0C5D398387")
                };

                await _unitOfWork.UserRepository.AddAsync(newUser);
            }
            _unitOfWork.Complete();
            newToken = CreateJwtToken(existingUser ?? newUser);

            return new AccessTokenModel { Success = true, Token = new JwtSecurityTokenHandler().WriteToken(newToken) };
        }
        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message.ToUpper());
        }
    }
    public async Task<SignUpResponse> SignupForCustomer(SignUpModel req)
    {
        try
        {
            if (!FunctionCommon.IsValidEmail(req.Email) || !FunctionCommon.IsValidPhoneNumber(req.PhoneNumber))
            {
                throw new BadRequestException("EMAIL HOẶC SỐ ĐIỆN THOẠI KHÔNG HỢP LỆ!");
            }
            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.Email == req.Email).FirstOrDefaultAsync();
            
            if (user != null)
            {
                if (user.IsVerified == true)
                {
                    return new SignUpResponse
                    {
                        Verified = null,
                        Messages = "EMAIL ĐÃ TỒN TẠI!"
                    };
                }
                else
                {
                    return new SignUpResponse
                    {
                        Verified = false,
                        Messages = "EMAIL ĐÃ ĐĂNG KÍ NHƯNG CHƯA ĐƯỢC XÁC THỰC!"
                    };
                }
            }

            var newUser = new User
            {
                UserID = Guid.NewGuid(),
                Email = req.Email,
                Password = SecurityUtil.Hash(req.Password),
                FullName = req.FullName,
                UserName = req.UserName,
                Address = req.Address,
                PhoneNumber = req.PhoneNumber,
                Status = SD.GeneralStatus.ACTIVE,
                IsVerified = false,
                Avatar = "https://firebasestorage.googleapis.com/v0/b/cloudfunction-yt-2b3df.appspot.com/o/AVATAR_DEFAULT%2Fdc5551cc-b063-45d8-86e0-84ec6b7d2af6?alt=media&token=8f897d9b-bc83-45e2-9102-f0056f93a914",
                RoleID = new Guid("E6E2FCD6-22F0-426B-A3A0-DD0C5D398387"),
            };

            await _unitOfWork.UserRepository.AddAsync(newUser);
            var res = _unitOfWork.Complete();

            if (res < 0)
            {
                return new SignUpResponse
                {
                    Verified = null,
                    Messages = "ĐĂNG KÍ THẤT BẠI!"
                };
            }

            return new SignUpResponse
            {
                Verified = true,
                Messages = "ĐĂNG KÍ THÀNH CÔNG, VUI LÒNG KIỂM TRA EMAIL VÀ XÁC NHẬN OTP!"
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    public async Task<SignUpResponse> SignUpForStaff(SignUpModel req)
    {
        try
        {
            if (!FunctionCommon.IsValidEmail(req.Email) || !FunctionCommon.IsValidPhoneNumber(req.PhoneNumber))
            {
                throw new BadRequestException("EMAIL HOẶC SỐ ĐIỆN THOẠI KHÔNG HỢP LỆ!");
            }
            var message = "";
            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.Email == req.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                return new SignUpResponse
                {
                    Verified = null,
                    Messages = "EMAIL ĐÃ TỒN TẠI!"
                };
            }

            var newUser = new User
            {
                UserID = Guid.NewGuid(),
                Email = req.Email,
                Password = SecurityUtil.Hash(req.Password),
                FullName = req.FullName,
                UserName = req.UserName,
                Address = req.Address,
                PhoneNumber = req.PhoneNumber,
                Status = SD.GeneralStatus.ACTIVE,
                IsVerified = true,
                Avatar = "https://firebasestorage.googleapis.com/v0/b/cloudfunction-yt-2b3df.appspot.com/o/AVATAR_DEFAULT%2Fdc5551cc-b063-45d8-86e0-84ec6b7d2af6?alt=media&token=8f897d9b-bc83-45e2-9102-f0056f93a914",
                RoleID = new Guid("9ADFF955-DB2B-4688-9048-30CC8367A519"),
                CompanyID = req.CompanyID,
            };

            await _unitOfWork.UserRepository.AddAsync(newUser);
            var res = _unitOfWork.Complete();

            if (res < 0)
            {
                return new SignUpResponse
                {
                    Verified = null,
                    Messages = "ĐĂNG KÍ THẤT BẠI!"
                };
            }
            return new SignUpResponse
            {
                Verified = true,
                Messages = "ĐĂNG KÍ THÀNH CÔNG!"
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> CreateManagementAccount(string email, string companyName)
        {
        try
        {
            if (!FunctionCommon.IsValidEmail(email))
            {
                throw new BadRequestException("EMAIL KHÔNG HỢP LỆ!");
            }
            var checkEmail = await _unitOfWork.UserRepository
                                              .FindByCondition(e => e.Email.Equals(email))
                                              .FirstOrDefaultAsync();
            if (checkEmail == null)
            {
                var password = GenerateRandomPassword();
                var mailData = new MailData
                {
                    EmailToId = email,
                    EmailToName = "TicketBookingWebSite",
                    EmailBody = SendAccountToManager(email, password, companyName),
                    EmailSubject = "TÀI KHOẢN CÔNG TY ĐÃ ĐĂNG KÍ TẠI WEBSITE THE BUS JOURNEY"
                };
                var emailResult = await _emailService.SendEmailAsync(mailData);
                if (!emailResult)
                {
                    return false;
                }
                var userId = Guid.NewGuid();
                var companyId = Guid.NewGuid();
                var user = new User
                {
                    UserID = userId,
                    Email = email,
                    Password = SecurityUtil.Hash(password),
                    Avatar = "https://firebasestorage.googleapis.com/v0/b/ticketbooking-427114.appspot.com/o/AVATAR_DEFAULT%2Fbb2cc7bf-b176-4518-88ef-0896b73f32e6?alt=media&token=05fbc03f-a08a-41ef-a746-f6177483d873",
                    CreateDate = DateTime.Now,
                    RoleID = new Guid("9ADBB896-AB5C-4688-9048-30CC8367A519"),
                    Status = SD.GeneralStatus.ACTIVE,
                    IsVerified = true
                };
                await _unitOfWork.UserRepository.AddAsync(user);
                _unitOfWork.Complete();
                var company = new Company
                {
                    CompanyID = companyId,
                    Name = companyName,
                    Status = SD.GeneralStatus.ACTIVE,
                    UserID = userId 
                };
                await _unitOfWork.CompanyRepository.AddAsync(company);
                _unitOfWork.Complete();
                user.CompanyID = companyId;
                _unitOfWork.UserRepository.Update(user);
                var rs = _unitOfWork.Complete();
                return rs > 0 ? true : false;
            }
            else
            {
                throw new InternalServerErrorException(SD.Notification.Existed("EMAIL","USER"));
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

    }

    private string GenerateRandomPassword()
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var random = new Random();
        return new string(Enumerable.Repeat(validChars, 8)
                                    .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    public async Task<bool> SendOtpToUser(string email, string fullName)
    {
        try
        {
            if (!FunctionCommon.IsValidEmail(email) )
            {
                throw new BadRequestException("EMAIL KHÔNG HỢP LỆ!");
            }
            var otp = new Random().Next(100000, 999999);
            var mailData = new MailData
            {
                EmailToId = email,
                EmailToName = "TicketBookingWebSite",
                EmailBody = GenerateEmailBody(fullName, otp),
                EmailSubject = "XÁC NHẬN MÃ OTP"
            };

            var emailResult = await _emailService.SendEmailAsync(mailData);
            if (!emailResult)
            {
                return false;
            }

            var createUser = new CreateUserReq
            {
                Email = email,
                OTPCode = otp.ToString(),
            };

            var createUserResponse = await _userService.SendOTPCode(createUser);

            if (createUserResponse.returnModel.OTPCode != otp.ToString())
            {
                var mailUpdateData = new MailData
                {
                    EmailToId = email,
                    EmailToName = "TicketBookingWebSite",
                    EmailBody = GenerateEmailBody(fullName, otp),
                    EmailSubject = "XÁC NHẬN MÃ OTP"
                };

                var rsUpdate = await _emailService.SendEmailAsync(mailUpdateData);
                if (!rsUpdate)
                {
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<LoginResponse> Login(string email, string password, string deviceToken)
    {
        try
        {
            if (!FunctionCommon.IsValidEmail(email))
            {
                throw new BadRequestException("EMAIL KHÔNG HỢP LỆ!");
            }
            var user = await _unitOfWork.UserRepository
                                        .FindByCondition(u => u.Email == email)
                                        .FirstOrDefaultAsync();
            var hash = SecurityUtil.Hash(password);

            if (user == null || !SecurityUtil.Hash(password).Equals(user.Password))
            {
                return new LoginResponse
                {
                    Verified = null,
                    Message = "EMAIL HOẶC PASSWORD KHÔNG ĐÚNG!"
                };
            }

            if (user.IsVerified == false)
            {
                return new LoginResponse
                {
                    Verified = false,
                    Message = "EMAIL ĐÃ ĐĂNG KÍ NHƯNG CHƯA XÁC THỰC!"
                };
            }

            var userRole = await _unitOfWork.UserRoleRepository
                                            .FindByCondition(ur => ur.RoleID == user.RoleID)
                                            .FirstOrDefaultAsync();
            user.UserRole = userRole!;
            if (deviceToken != null)
            {
                user.AccessToken = deviceToken;
            }
            await _unitOfWork.UserRepository.Commit();
            return new LoginResponse
            {
                Authenticated = true,
                Token = CreateJwtToken(user),
                Verified = user.IsVerified,
                Message = "ĐĂNG NHẬP THÀNH CÔNG",
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public SecurityToken CreateJwtToken(User user)
    {
        try
        {
            var utcNow = DateTime.UtcNow;
            var userRole = _unitOfWork.UserRoleRepository.FindByCondition(u => u.RoleID == user.RoleID).FirstOrDefault();
            var authClaims = new List<Claim>
          {
              new(JwtRegisteredClaimNames.NameId, user.UserID.ToString()),
              new(JwtRegisteredClaimNames.Sub, user.UserName),
              new(JwtRegisteredClaimNames.Email, user.Email),
              new(ClaimTypes.Role, userRole.RoleName),
              new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          };

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(authClaims),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Expires = utcNow.Add(TimeSpan.FromHours(1)),
            };

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(tokenDescriptor);

            return token;
        }
        catch (Exception ex)
        {
            throw new BadRequestException("LỖI XẢY RA KHI TẠO MỚI TOKEN!");
        }
    }

    private string GenerateEmailBody(string fullName, int otp)
    {
        return $@"
   <body style=""display: flex; justify-content: center; align-items: center"">
    <div>
      <div
        style=""
          color: #536e88;
          width: fit-content;
          box-shadow: 0 2px 8px rgba(8, 120, 211, 0.2);
          padding: 10px;
          border-radius: 5px;
        ""
      >
        <div
          style=""
            display: flex;
            justify-content: center;
            align-items: center;
            height: 10px;
            margin-top: 0px;
            background-color: #3498db;
            font-size: 0.875rem;
            font-weight: bold;
            color: #ffffff;
          ""
        ></div>

        <h1 style=""text-align: center; color: #3498db"">
          Chào mừng đến với
          <span style=""color: #f99f41"">trạm của chúng tôi!</span>
        </h1>

        <div style=""text-align: center"">
          <img
            src=""https://img.freepik.com/free-vector/students-bus-transportation_24877-83765.jpg?size=338&ext=jpg&ga=GA1.1.553209589.1715040000&semt=ais""
            alt=""logo""
            width=""70""
          />
        </div>

        <p style=""text-align: center; font-weight: bold; margin-top: 0"">
          <span style=""color: #f99f41"">THE BUS </span
          ><span style=""color: #3498db"">JOURNEY</span>
        </p>

        <div
          style=""
            width: fit-content;
            margin: auto;
            box-shadow: 0 2px 8px rgba(8, 120, 211, 0.2);
            padding-top: 10px;
            border-radius: 10px;
          ""
        >
          <p>
            Xin chào,
            <span style=""font-weight: bold; color: #0d1226"">{fullName}</span>
          </p>
          <p>
            <span style=""font-weight: bold"">THE BUS JOURNEY </span>xin thông báo
            tài khoản của bạn đã được đăng kí thành công. <span></span>
          </p>
          <p>
            <span>Mã xác thực của bạn là: </span
            ><span style=""color: #0d1226; font-weight: bold"">{otp}</span>
          </p>
          <p>Xin chân thành cảm ơn vì bạn đã sử dụng dịch vụ của chúng tôi!</p>
          <p>Hân hạnh,</p>
          <p style=""font-weight: 700; color: #0d1226"">THE BUS JOURNEY</p>
        </div>

        <div
          style=""
            display: flex;
            justify-content: center;
            align-items: center;
            height: 40px;
            background-color: #3498db;
            font-size: 0.875rem;
            font-weight: bold;
            color: #ffffff;
          ""
        >
          © 2024 | Bản quyền thuộc về THE BUS JOURNEY.
        </div>
      </div>
    </div>
  </body>

    ";
    }
    private string SendAccountToManager(string email, string password, string companyName)
    {
        return $@"<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f6f6f6;"">
    <div style=""max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);"">
        <div class=""header"">
            <div style=""text-align: center"">
                <img
                  src=""https://img.freepik.com/free-vector/students-bus-transportation_24877-83765.jpg?size=338&ext=jpg&ga=GA1.1.553209589.1715040000&semt=ais""
                  alt=""logo""
                  width=""70""
                />
            </div>
            <p style=""text-align: center; font-weight: bold; margin-top: 0"">
                <span style=""color: #f99f41"">THE BUS </span>
                <span style=""color: #3498db"">JOURNEY</span>
            </p>
        </div>
        <div  style=""padding: 5px 20px 20px;"">
            <p>Xin chào quí khách !</p>
            <p>Bạn đã thành công trong việc đăng kí Website <strong>The Bus Journey.</strong> </p>
            <p>Với tên công ty là : <strong style=""font-size: 18px; color:#f99f41;"">{companyName}.</strong>
            <p>Dưới đây là tài khoản của bạn: </p>
            <p>Email: <strong>{email}</strong></p>
            <p>Mật khẩu: <strong>{password}</strong></p>
            <p>Vui lòng giữ kín thông tin này và không được chia sẻ đến bất cứ ai, nếu có chuyện gì xảy ra ngoài ý muốn <strong>The Bus Journey</strong> sẽ không chịu trách nhiệm.</p>
            <p>Xin cảm ơn !</p>
        </div>
        <div style=""text-align: center; padding: 10px; font-size: 12px; color: #777777;"">
            <p>Cảm ơn bạn đã chọn dịch vụ của chúng tôi !</p>
            <p>Vui lòng truy cập vào Website <strong>The Bus Journey</strong> để tiếp tục !</p>
            <a href=""https://admin-bus-journey.vercel.app/?fbclid=IwZXh0bgNhZW0CMTAAAR3s1jlKgf1Vzv6Ypt6YQ5s9iE3O3jnsln30ECpErINLUgrJS8CbUHoQ7-A_aem_V25RoaCRILDky_Gk7TYrNg"">https://admin-bus-journey.vercel.app</a>
        </div>
    </div>
</body>";
    }
}