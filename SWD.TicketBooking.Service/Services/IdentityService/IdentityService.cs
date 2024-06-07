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
using SWD.TicketBooking.Repo.Exceptions;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services.FirebaseService;


namespace SWD.TicketBooking.Service.Services.IdentityService;

public class IdentityService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IRepository<User, int> _userRepository;
    private readonly IRepository<UserRole, int> _userRoleRepository;
    private readonly IFirebaseService _firebaseService;

    public IdentityService(IOptions<JwtSettings> jwtSettingsOptions, IRepository<User, int> userRepository, IRepository<UserRole, int> userRoleRepository, IFirebaseService firebaseService)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettingsOptions.Value;
        _userRoleRepository = userRoleRepository;
        _firebaseService = firebaseService;
    }

    public async Task<bool> Signup(SignUpModel req)
    {     
            try
            {
             
                var user = await _userRepository.FindByCondition(u => u.Email == req.Email).FirstOrDefaultAsync();
                if (user is not null)
                {
                    throw new BadRequestException("username or email already exists");
                }

            var userAdd = await _userRepository.AddAsync(new User
            {
                Email = req.Email,
                Password = SecurityUtil.Hash(req.Password),
                FullName = req.FullName,
                UserName = req.UserName,
                Address = req.Address,
                PhoneNumber = req.PhoneNumber,
                Status = "Active",
                IsVerified = false,
                UrlGuidID = "",
                Avatar = "https://res.cloudinary.com/dkdl8asci/image/upload/v1711506064/canhcut_zpazas.webp?fbclid=IwZXh0bgNhZW0CMTAAAR27ufM-uhy8i9s-S-aAXmlIyJEt2-qP9EUhcXMzP9TSbdyoA4ifW-t4zzk_aem_AbJfJkMqTauRCYn09gIF1SWycsbwalv7be8u-ufHN4nWqlVljdcG-DAPaC1w0B7RieBjNDYOXJ_mzsLOS4Th4rTQ",
                RoleID = 2,
            }) ; ;
                var res = await _userRepository.Commit();
              /*  var imagePath = FirebasePathName.AVATAR + $"{userAdd.UrlGuidID}";
                var imageUploadResult = await _firebaseService.UploadFileToFirebase("https://res.cloudinary.com/dkdl8asci/image/upload/v1711506064/canhcut_zpazas.webp?fbclid=IwZXh0bgNhZW0CMTAAAR27ufM-uhy8i9s-S-aAXmlIyJEt2-qP9EUhcXMzP9TSbdyoA4ifW-t4zzk_aem_AbJfJkMqTauRCYn09gIF1SWycsbwalv7be8u-ufHN4nWqlVljdcG-DAPaC1w0B7RieBjNDYOXJ_mzsLOS4Th4rTQ", imagePath);
                if (imageUploadResult.IsSuccess)
                    {
                        userAdd.Avatar = (string)imageUploadResult.Result;
                    }

                 _userRepository.Update(userAdd);
                var rs = await _userRepository.Commit();*/
            return res > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
    }

    public async Task<LoginResponse> Login(string email, string password)
    {
        var user = await  _userRepository.FindByCondition(u => u.Email == email && u.IsVerified == true).FirstOrDefaultAsync();


        if (user is null)
        {
            return new LoginResponse
            {
                Authenticated = false,
                Token = null,
            };
        }
        var userRole = _userRoleRepository.FindByCondition(ur => ur.RoleID == user.RoleID).FirstOrDefault();

        user.UserRole = userRole!;

        var hash = SecurityUtil.Hash(password);
        if (!user.Password.Equals(hash))
        {
            return new LoginResponse
            {
                Authenticated = false,
                Token = null,
            };
        }

        return new LoginResponse
        {
            Authenticated = true,
            Token = CreateJwtToken(user),
        };
    }

    private SecurityToken CreateJwtToken(User user)
    {
        try
        {
            var utcNow = DateTime.UtcNow;
            var userRole = _userRoleRepository.FindByCondition(u => u.RoleID == user.RoleID).FirstOrDefault();
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