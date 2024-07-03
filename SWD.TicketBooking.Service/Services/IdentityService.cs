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

namespace SWD.TicketBooking.Service.Services;

public class IdentityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;
    private readonly IFirebaseService _firebaseService;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public IdentityService(IUserService userService, IEmailService emailService, IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettingsOptions, IFirebaseService firebaseService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _emailService = emailService;
        _jwtSettings = jwtSettingsOptions.Value;
        _firebaseService = firebaseService;
    }

    public async Task<SignUpResponse> Signup(SignUpModel req)
    {
        try
        {
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

    public async Task<bool> SendOtpToUser(string email, string fullName)
    {
        try
        {
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

    public async Task<LoginResponse> Login(string email, string password)
    {
        try
        {
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

    private SecurityToken CreateJwtToken(User user)
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
}