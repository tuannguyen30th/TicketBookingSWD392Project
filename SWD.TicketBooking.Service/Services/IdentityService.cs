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


namespace SWD.TicketBooking.Service.Services;

public class IdentityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;
    //private readonly IRepository<User, int> _unitOfWork.UserRepository;
    //private readonly IRepository<UserRole, int> _unitOfWork.UserRoleRepository;
    private readonly IFirebaseService _firebaseService;

    public IdentityService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettingsOptions, IRepository<User, int> userRepository, IRepository<UserRole, int> userRoleRepository, IFirebaseService firebaseService)
    {
        _unitOfWork = unitOfWork;
        //_unitOfWork.UserRepository = userRepository;
        _jwtSettings = jwtSettingsOptions.Value;
        //_unitOfWork.UserRoleRepository = userRoleRepository;
        _firebaseService = firebaseService;
    }

    public async Task<bool> Signup(SignUpModel req)
    {
        try
        {

            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.Email == req.Email).FirstOrDefaultAsync();
            if (user is not null)
            {
                throw new BadRequestException("username or email already exists");
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
                Status = "Active",
                IsVerified = false,
                Avatar = "https://firebasestorage.googleapis.com/v0/b/cloudfunction-yt-2b3df.appspot.com/o/AVATAR_DEFAULT%2Fdc5551cc-b063-45d8-86e0-84ec6b7d2af6?alt=media&token=8f897d9b-bc83-45e2-9102-f0056f93a914",
                RoleID = new Guid("E6E2FCD6-22F0-426B-A3A0-DD0C5D398387"),
            });
            //var res = await _unitOfWork.UserRepository.Commit();
            var res = _unitOfWork.Complete();
            /*  var imagePath = FirebasePathName.AVATAR + $"{userAdd.UrlGuidID}";
              var imageUploadResult = await _firebaseService.UploadFileToFirebase("https://res.cloudinary.com/dkdl8asci/image/upload/v1711506064/canhcut_zpazas.webp?fbclid=IwZXh0bgNhZW0CMTAAAR27ufM-uhy8i9s-S-aAXmlIyJEt2-qP9EUhcXMzP9TSbdyoA4ifW-t4zzk_aem_AbJfJkMqTauRCYn09gIF1SWycsbwalv7be8u-ufHN4nWqlVljdcG-DAPaC1w0B7RieBjNDYOXJ_mzsLOS4Th4rTQ", imagePath);
              if (imageUploadResult.IsSuccess)
                  {
                      userAdd.Avatar = (string)imageUploadResult.Result;
                  }

               _unitOfWork.UserRepository.Update(userAdd);
              var rs = await _unitOfWork.UserRepository.Commit();*/
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
            var user = await _unitOfWork.UserRepository.FindByCondition(u => u.Email == email).FirstOrDefaultAsync();
            var hash = SecurityUtil.Hash(password);
            if (user != null && user.Password.Equals(hash) && user.IsVerified == false)
            {
                return new LoginResponse
                {
                    Verified = user.IsVerified,
                    Message = "Email đã đăng kí nhưng chưa xác thực!"
                };
            }
            if (user is null)
            {
                throw new NotFoundException("Không tìm thấy user!");
            }

            var userRole = await _unitOfWork.UserRoleRepository.FindByCondition(ur => ur.RoleID == user.RoleID).FirstOrDefaultAsync();
            user.UserRole = userRole!;

            if (!user.Password.Equals(hash))
            {
                return new LoginResponse
                {
                    Verified = user.IsVerified,
                    Message = "Email hoặc Password không đúng!"
                };
            }

            return new LoginResponse
            {
                Authenticated = true,
                Token = CreateJwtToken(user),
                Verified = user.IsVerified,
                Message = "Đăng nhập thành công"
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
            throw new BadRequestException("l?i");
        }
    }


}