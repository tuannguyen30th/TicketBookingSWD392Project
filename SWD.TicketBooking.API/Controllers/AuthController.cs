using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.API.Common;
using AutoMapper;
using SWD.TicketBooking.Service.Exceptions;
using Microsoft.AspNetCore.Identity;
using SWD.TicketBooking.Service.Services;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.API.RequestModels;
using Google.Apis.Auth;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
using SWD.TicketBooking.Repo.Entities;
using Google.Apis.Http;
using SWD.TicketBooking.Service.Utilities;
using SWD.TicketBooking.Repo.UnitOfWork;

namespace SWD.TicketBooking.Booking.API;

[Route("auth-management")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IdentityService _identityService;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private static readonly HttpClient httpClient = new HttpClient();
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration, IdentityService identityService, IUserService userService, IEmailService emailService, IMapper mapper)
    {
        _identityService = identityService;
        _userService = userService;
        _emailService = emailService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost("managed-auths/sign-ups")]
    public async Task<IActionResult> Signup([FromBody] SignUpRequest req)
    {
        try
        {
            var res = await _identityService.Signup(_mapper.Map<SignUpModel>(req));
            if (!res)
            {
                var resultFail = new SignUpResponse
                {
                    Messages = "Sign up fail"
                };
                return BadRequest(resultFail);
            }

            var resultSucess = new SignUpResponse
            {
                Messages = "Sign up success and check your email and verify the OTP!"
            };

            var userResponse = await _userService.GetUserByEmailForOTP(req.Email);

            if (userResponse == null)
            {
                return BadRequest();
            }

            if (userResponse.OTPCode == "0" && userResponse.IsVerified == true)
            {
                return BadRequest();
            }

            if (userResponse.IsVerified == false)
            {
                var otp = new Random().Next(100000, 999999);

                var mailData = new MailData
                {
                    EmailToId = req.Email,
                    EmailToName = "TicketBookingWebSite",
                    EmailBody = GenerateEmailBody(userResponse.FullName, otp),
                    EmailSubject = "OTP Verification"
                };

                var emailResult = await _emailService.SendEmailAsync(mailData);
                if (!emailResult)
                {
                    throw new BadRequestException("Failed to send email.");
                }

                var createUser = new CreateUserReq
                {
                    Email = req.Email,
                    OTPCode = otp.ToString(),
                };

                var createUserResponse = await _userService.SendOTPCode(createUser);

                if (createUserResponse.returnModel.OTPCode != otp.ToString())
                {
                    var mailUpdateData = new MailData
                    {
                        EmailToId = req.Email,
                        EmailToName = "TicketBookingWebSite",
                        EmailBody = GenerateEmailBody(userResponse.FullName, otp),
                        EmailSubject = "OTP Verification"
                    };

                    var rsUpdate = await _emailService.SendEmailAsync(mailUpdateData);
                    if (!rsUpdate)
                    {
                        return BadRequest();
                    }
                }
                return Ok(resultSucess);
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
        }
    }
    [AllowAnonymous]
    [HttpPost("managed-auths/sign-ins")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var loginResult = await _identityService.Login(req.Email, req.Password);
        if (!loginResult.Authenticated)
        {          
            return BadRequest(loginResult);
        }
        
        var handler = new JwtSecurityTokenHandler();
        var res = new SWD.TicketBooking.API.ResponseModels.LoginResponse
        {
            AccessToken = handler.WriteToken(loginResult.Token),
        };

        if (loginResult.CompanyID != null)
        {
            res = new SWD.TicketBooking.API.ResponseModels.LoginResponse
            {
                AccessToken = handler.WriteToken(loginResult.Token),
                CompanyID = loginResult.CompanyID
            };
        }

        return Ok(res);
    }
    [HttpPost("managed-auths/access-token-verification")]
    public async Task<IActionResult> CheckAccessToken([FromBody] string accessToken)
    {
        try
        {
            var existingUser = await _userService.GetUserByAccessToken(accessToken);
            if (existingUser != null)
            {
                if (!existingUser.IsTokenExpired())
                {
                    var userResult = new
                    {
                        Id = existingUser.UserID,
                        Email = existingUser.Email,
                        Name = existingUser.FullName,
                        Picture = existingUser.Avatar
                    };

                    return Ok(new { success = true, userInfo = userResult });
                }
                else
                {
                    throw new InternalServerErrorException("Access token has expired. Please log in again.".ToUpper());
                }
            }
            var tokenInfoUrl = $"https://www.googleapis.com/oauth2/v3/tokeninfo?access_token={accessToken}";
            var response = await httpClient.GetAsync(tokenInfoUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new BadRequestException(errorContent.ToUpper());
            }

            var tokenInfo = await response.Content.ReadAsStringAsync();
            var userInfoUrl = $"https://www.googleapis.com/oauth2/v1/userinfo?access_token={accessToken}";
            var userInfoResponse = await httpClient.GetAsync(userInfoUrl);

            if (!userInfoResponse.IsSuccessStatusCode)
            {
                var userInfoError = await userInfoResponse.Content.ReadAsStringAsync();
                throw new BadRequestException(userInfoError.ToUpper());
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

            existingUser = await _userService.GetUserByEmail2(userResultNew.Email);
            if (existingUser != null)
            {
                existingUser.AccessToken = accessToken;
                existingUser.TokenExpiration = DateTime.UtcNow.AddHours(1);
                _unitOfWork.UserRepository.Update(existingUser);
            }
            else
            {
                var newUser = new User
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
                    AccessToken = accessToken,
                    TokenExpiration = DateTime.UtcNow.AddHours(1),
                    RoleID = new Guid("E6E2FCD6-22F0-426B-A3A0-DD0C5D398387")
                };

                await _unitOfWork.UserRepository.AddAsync(newUser);
            }
            _unitOfWork.Complete();

            return Ok(new { success = true, tokenInfo = tokenInfo, userInfo = userResultNew });
        }

        catch (Exception ex)
        {
            throw new BadRequestException(ex.Message.ToUpper());
        }
    }
    [Authorize]
    [HttpGet("managed-auths/token-verification")]
    public async Task<IActionResult> CheckToken()
    {
        Request.Headers.TryGetValue("Authorization", out var token);
        token = token.ToString().Split()[1];
        // Here goes your token validation logic
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new BadRequestException("Authorization header is missing or invalid.");
        }
        // Decode the JWT token
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Check if the token is expired
        if (jwtToken.ValidTo < DateTime.UtcNow)
        {
            throw new BadRequestException("Token has expired.");
        }

        string email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        var user = await _userService.GetUserByEmail(email);

        if (user == null)
        {
            return BadRequest("email is in valid");
        }

        // If token is valid, return success response
        return Ok(ApiResult<CheckTokenResponse>.Succeed(new CheckTokenResponse
        {
            User = user,
        }));
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