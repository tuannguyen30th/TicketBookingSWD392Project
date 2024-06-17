using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Service.Dtos.User;
using SWD.TicketBooking.Service.Dtos.Auth;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common;
using SWD.TicketBooking.API.Common.ResponseModels;
using AutoMapper;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Services;
using SWD.TicketBooking.Service.IServices;

namespace SWD.TicketBooking.Controllers
{
    [Route("user")]
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

        [HttpGet("user-detail/{userID}")]
        public async Task<IActionResult> GetUserDetail([FromRoute] Guid userID)
        {
            var user = _mapper.Map<UserResponse>(await _userService.GetUserById(userID));
            return Ok(user);
        }

        [HttpPut("user/{userID}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userID, [FromBody] UpdateUserRequest req)
        {
            var map = _mapper.Map<UpdateUserModel>(req);
            var user = await _userService.UpdateUser(userID, map);
            return Ok(user);
        }

        [HttpPost("send-otp-code")]
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
                        Messages = "Account does not exist!"
                    };
                    return BadRequest(resultFail);
                }

                if (userResponse.OTPCode == "0" && userResponse.IsVerified == true)
                {

                    resultFail = new SignUpResponse
                    {
                        Messages = "Account does not exist!"
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
                                Messages = "Failed to send update email."
                            });

                        }
                    }

                    return Ok(new SignUpResponse
                    {
                        Messages = "Check your email and verify the OTP."
                    });
                }
                resultFail = new SignUpResponse
                {
                    Messages = "Error."
                };
                return BadRequest(resultFail);
            }

            catch (Exception ex)
            {

                var resultFail = new SignUpResponse
                {
                    Messages = "An error occurred while sending OTP."
                };
                return BadRequest(resultFail);
            }
        }
        [HttpPut("submit-otp")]
        public async Task<IActionResult> SubmitOTP(SubmitOTPReq req)
        {
            try
            {
                var checkOTP = await _userService.SubmitOTP(req);
                if (checkOTP.returnModel == null)
                {
                    return BadRequest(new GenericResponse<UserModel>
                    {
                        Data = null,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = checkOTP.message
                    });

                }
                return Ok(new GenericResponse<UserModel>
                {
                    Data = checkOTP.returnModel,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = checkOTP.message
                });
            }
            catch (Exception ex) {
                return BadRequest(new GenericResponse<UserModel>
                {
                    Data = null,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }

        }

        [HttpGet("all-users")]
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
        private string BookingSend()
        {
            return $@"<body style=""font-size: 14px; background: #f5f5f5; padding: 20px;"">
  <h1 style=""text-align: center;"">Xác nhận hoàn thành đặt vé</h1>
  <table style=""width: 100%; max-width: 600px; margin: 0 auto; background: white; box-shadow: rgba(0, 0, 0, 0.3) 0px 19px 38px, rgba(0, 0, 0, 0.22) 0px 15px 12px; border-collapse: collapse;"">
    <tr>
      <td style=""width: 50%; vertical-align: top; border-right: 1px dashed #404040; padding: 20px;"">
        <div style=""margin-bottom: 20px;"">
          <p style=""font-size: large; font-weight: 600;"">Giá vé: <span style=""font-size: x-large; font-weight: 600;color: #ea7019;"">170.000đ</span></p>
          <p style=""font-size: medium; font-weight: 600;"">Giá dịch vụ:</p>
        </div>
        <p style=""font-size: medium; margin: 0;"">
          <span style=""color: dimgray;"">Dịch vụ 1:</span>
          <span style=""font-weight: bold;color: #ea7019;"">20.000đ</span>
          <span style=""font-style: italic; font-weight: bold;"">Trạm A</span>
        </p>
        <p style=""font-size: medium; margin: 0;"">
          <span style=""color: dimgray;"">Dịch vụ 2:</span>
          <span style=""font-weight: bold;color: #ea7019;"">30.000đ</span>
          <span style=""font-style: italic; font-weight: bold;"">Trạm B</span>
        </p>
        <p style=""font-size: medium; margin: 0;"">
          <span style=""color: dimgray;"">Dịch vụ 3:</span>
          <span style=""font-weight: bold;color: #ea7019;"">30.000đ</span>
          <span style=""font-style: italic; font-weight: bold;"">Trạm C</span>
        </p>
      </td>
      <td style=""width: 50%; padding: 20px; text-align: center;"">
        <p style=""border-top: 1px solid gray; border-bottom: 1px solid gray; padding: 5px 0; font-weight: 700; margin: 20px 0;"">
          <span style=""color: #ea7019;"">THE BUS JOURNEY</span>
        </p>
        <div>
          <h3>Nguyễn Ngọc Quân</h3>
          <h4>Chặng đi: Hà Nội - Bến Tre</h4>
        </div>
        <div>
          <p>Khởi hành: <span style=""font-size: larger; font-weight: 700"">15:30</span></p>
          <p>Ngày: <span style=""font-size: larger; font-weight: 700"">24/07/2023</span></p>
        </div>
        <p>Vị trí vé: <span style=""font-size: larger; font-weight: 700"">A15</span></p>
      </td>
    </tr>
    <tr>
      <td colspan=""2"" style=""padding: 20px; text-align: center; background: #F5B642;"">
        <h1 style=""font-size: 18px;"">Tổng hóa đơn</h1>
        <h1>650.000đ</h1>
        <div style=""height: 100px; margin: 20px 0;"">
          <img src=""https://external-preview.redd.it/cg8k976AV52mDvDb5jDVJABPrSZ3tpi1aXhPjgcDTbw.png?auto=webp&s=1c205ba303c1fa0370b813ea83b9e1bddb7215eb"" alt=""QR code"" style=""height: 100%;"" />
        </div>
        <p>Cảm ơn quý khách đã tin tưởng</p>
      </td>
    </tr>
  </table>
</body>
";
        } 

    }
}
