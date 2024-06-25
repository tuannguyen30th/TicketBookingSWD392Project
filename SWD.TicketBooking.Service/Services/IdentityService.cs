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


namespace SWD.TicketBooking.Service.Services;

public class IdentityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;
    private readonly IFirebaseService _firebaseService;

    public IdentityService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettingsOptions, IFirebaseService firebaseService)
    {
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtSettingsOptions.Value;
        _firebaseService = firebaseService;
    }

    public async Task<bool> Signup(SignUpModel req)
    {
        try
        {

            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.Email == req.Email).FirstOrDefaultAsync();
            if (user is not null)
            {
                throw new BadRequestException("Email đã tồn tại!".ToUpper());
            }

            var userAdd = await _unitOfWork.UserRepository.AddAsync(new User
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
            });
            var res = _unitOfWork.Complete();
            return res > 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
            if (user != null && user.Password.Equals(hash) && user.IsVerified == false)
            {
                return new LoginResponse
                {
                    Verified = user.IsVerified,
                    Message = "Email đã đăng kí nhưng chưa xác thực!".ToUpper()
                };
            }
            if (user is null)
            {
                throw new NotFoundException(SD.Notification.NotFound("Người dùng"));
            }

            var userRole = await _unitOfWork.UserRoleRepository
                                            .FindByCondition(ur => ur.RoleID == user.RoleID)
                                            .FirstOrDefaultAsync();
            user.UserRole = userRole!;

            if (!user.Password.Equals(hash))
            {
                return new LoginResponse
                {
                    Verified = user.IsVerified,
                    Message = "Email hoặc Password không đúng!".ToUpper()
                };
            }

            return new LoginResponse
            {
                Authenticated = true,
                Token = CreateJwtToken(user),
                Verified = user.IsVerified,
                Message = "Đăng nhập thành công".ToUpper()
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
            throw new BadRequestException("Lỗi xảy ra khi tạo mới Token!".ToUpper());
        }
    }


}