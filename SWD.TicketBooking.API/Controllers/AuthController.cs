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
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.SeedData;

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
            var signUpResponse = await _identityService.Signup(_mapper.Map<SignUpModel>(req));

            if (signUpResponse.Verified == true)
            {
                var userResponse = await _userService.GetUserByEmailForOTP(req.Email);
                if (userResponse == null)
                {
                    return BadRequest();
                }

                if (userResponse.IsVerified == false)
                {
                    var otpSent = await _identityService.SendOtpToUser(req.Email, userResponse.FullName);

                    if (!otpSent)
                    {
                        return BadRequest(new { Message = "GỬI EMAIL THẤT BẠI!" });
                    }

                    return Ok(new { Message = "ĐĂNG KÍ THÀNH CÔNG, VUI LÒNG KIỂM TRA EMAIL VÀ XÁC NHẬN OTP!" });
                }
            }

            return Ok(signUpResponse);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
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
                    throw new InternalServerErrorException("MÃ THÔNG BÁO TRUY CẬP ĐÃ HẾT HẠN. XIN VUI LÒNG ĐĂNG NHẬP LẠI!");
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
            throw new BadRequestException("AUTHORIZATION HEADER IS MISSING OR INVALID!");
        }
        // Decode the JWT token
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Check if the token is expired
        if (jwtToken.ValidTo < DateTime.UtcNow)
        {
            throw new BadRequestException("TOKEN HAS EXPIRED!");
        }

        string email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        var user = await _userService.GetUserByEmail(email);

        if (user == null)
        {
            return BadRequest("EMAIL KHÔNG TỒN TẠI!");
        }

        // If token is valid, return success response
        return Ok(ApiResult<CheckTokenResponse>.Succeed(new CheckTokenResponse
        {
            User = user,       
        }));
    }

    
}