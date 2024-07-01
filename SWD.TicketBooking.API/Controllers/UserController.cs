using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Service.Dtos.User;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.API.Common;
using AutoMapper;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Services;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using MimeKit.Encodings;

namespace SWD.TicketBooking.Controllers
{
    [Route("user-management")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IdentityService _identityService;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IdentityService identityService, IEmailService emailService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _userService = userService;
            _identityService = identityService;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;

        }
        [HttpGet("managed-users/staff/{companyID}")]
        public async Task<IActionResult> GetStaffFromCompany(Guid companyID)
        {
            var staff = _mapper.Map<List<GetStaffFromCompanyResponse>>(await _userService.GetStaffFromCompany(companyID));
            return Ok(staff);
        }
        [HttpPost("managed-users/avatars")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var rs = await _userService.UploadAvatar(file);
            return Ok(rs);
        }
        [HttpGet("managed-users/{userID}/details")]
        public async Task<IActionResult> GetUserDetail([FromRoute] Guid userID)
        {
            var user = _mapper.Map<UserResponse>(await _userService.GetUserById(userID));
            return Ok(user);
        }

        [HttpPut("managed-users/{userID}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userID, [FromForm] UpdateUserRequest req)
        {
            var map = _mapper.Map<UpdateUserModel>(req);
            var user = await _userService.UpdateUser(userID, map);
            return Ok(user);
        }

        [HttpPost("managed-users/otp-code-sending")]
        public async Task<IActionResult> SendOTPCodeToEmail(SendOTPCodeEmailReq req)
        {
            try
            {
                var resultFail = new SignUpResponse();

                var userResponse = await _userService.GetUserByEmailForOTP(req.Email);

                if (userResponse == null)
                {

                    resultFail = new SignUpResponse
                    {
                        Messages = "KHÔNG TỒN TẠI TÀI KHOẢN!"
                    };
                    return BadRequest(resultFail);
                }

                if (userResponse.OTPCode == "0" && userResponse.IsVerified == true)
                {

                    resultFail = new SignUpResponse
                    {
                        Messages = "KHÔNG TỒN TẠI TÀI KHOẢN!"
                    };
                    return BadRequest(resultFail);
                }

                if (userResponse.IsVerified == false)
                {
                    var otp = new Random().Next(100000, 999999);

                    var mailData = new MailData()
                    {
                        EmailToId = req.Email,
                        EmailToName = "TicketBookingWebSite",
                        EmailBody = GenerateEmailBody(userResponse.FullName, otp),
                        EmailSubject = "OTP Verification"
                    };

                    var emailResult = await _emailService.SendEmailAsync(mailData);
                    if (!emailResult)
                    {
                        throw new BadRequestException("LỖI XẢY RA KHI GỬI EMAIL!");
                    }

                    var createUser = new CreateUserReq
                    {
                        Email = req.Email,
                        OTPCode = otp.ToString(),
                    };

                    var createUserResponse = await _userService.SendOTPCode(createUser);

                    if (createUserResponse.returnModel.OTPCode != otp.ToString())
                    {
                        var mailUpdateData = new MailData()
                        {
                            EmailToId = req.Email,
                            EmailToName = "TicketBookingWebSite",
                            EmailBody = GenerateEmailBody(userResponse.FullName, otp),
                            EmailSubject = "OTP Verification"
                        };

                        var rsUpdate = await _emailService.SendEmailAsync(mailUpdateData);
                        if (!rsUpdate)
                        {
                            return BadRequest(new SignUpResponse
                            {
                                Messages = "LỖI XẢY RA KHI GỬI EMAIL!"
                            }) ;

                        }
                    }
                    return Ok(new SignUpResponse
                    {
                        Messages = "KIỂM TRA EMAIL CỦA BẠN VÀ XÁC NHẬN OTP!"
                    });
                }
                resultFail = new SignUpResponse
                {
                    Messages = "LỖI!"
                };
                return BadRequest(resultFail);
            }

            catch (Exception ex)
            {

                var resultFail = new SignUpResponse
                {
                    Messages = "LỖI KHI GỬI MÃ OTP!"
                };
                return BadRequest(resultFail);
            }
        }
        [HttpPut("managed-users/otp-code-submission")]
        public async Task<IActionResult> SubmitOTP(SubmitOTPReq req)
        {
                var checkOTP = await _userService.SubmitOTP(req);
                return Ok(checkOTP);
        }

        [HttpGet("managed-users")]
        public async Task<IActionResult> GetALlUsers()
        {
            var user = await _userService.GetAllUsers();
            var rs = _mapper.Map<List<UserResponse>>(user);
            return Ok(rs);
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
            <span style=""font-weight: bold; color: #0d1226"">{fullName}!</span>
          </p>
          <p>
            <span style=""font-weight: bold"">THE BUS JOURNEY </span>xin thông báo
            tài khoản của bạn đã được đăng kí thành công. <span></span>
          </p>
          <p>
            <span>Mã xác thực của bạn là: </span
            ><span style=""color: #0d1226; font-weight: bold; font-size: 18px;"">{otp}</span>
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
}
