using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Booking;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;
using System.Text;
using static SWD.TicketBooking.Service.Dtos.SendMailBookingModel;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("booking-management")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookingService _bookingService;
        private readonly IEmailService _emailService;

        public BookingController(IEmailService emailService, IBookingService bookingService, IMapper mapper)
        {
            _mapper = mapper;
            _emailService = emailService;
            _bookingService = bookingService;
        }

        [HttpPost("managed-bookings")]
        public async Task<IActionResult> AddOrUpdateBooking(BookingModel bookingRequest)
        {
          /*  var map = _mapper.Map<BookingModel>(bookingRequest);*/
            var rs = await _bookingService.AddOrUpdateBooking(bookingRequest, HttpContext);
            return Ok(rs);
        }

        [HttpGet("managed-bookings/vnpay-ipn")]
        public async Task<IActionResult> VNPayIPN()
        {
            try
            {
                var response = new VNPayModel
                {
                    PaymentMethod = Request.Query["vnp_BankCode"],
                    BookingDescription = Request.Query["vnp_OrderInfo"],
                    BookingId = Request.Query["vnp_TxnRef"],
                    PaymentId = Request.Query["vnp_TransactionNo"],
                    TransactionId = Request.Query["vnp_TransactionNo"],
                    Token = Request.Query["vnp_SecureHash"],
                    VnPayResponseCode = Request.Query["vnp_ResponseCode"],
                    PayDate = Request.Query["vnp_PayDate"],
                    Amount = Request.Query["vnp_Amount"],
                    Success = true
                };

                if (response.VnPayResponseCode == "00")
                {
                    Guid bookingId;
                    if (Guid.TryParse(response.BookingId, out bookingId))
                    {
                        var result = await _bookingService.UpdateStatusBooking(bookingId);
                        var getEmail = await _bookingService.GetEmailBooking(bookingId);
                        var mailUpdateData = new MailData()
                        {
                            EmailToId = getEmail.Value,
                            EmailToName = "TicketBookingWebSite",
                            EmailBody = BookingSend(result),
                            EmailSubject = "Ticket Information!"
                        };

                        var rsUpdate = await _emailService.SendEmailAsync(mailUpdateData);
                        if (!rsUpdate)
                        {
                            return BadRequest("Something wrong email!");
                        }
                    }
                    else
                    {
                        return BadRequest("Invalid booking ID format.");
                    }
                    return Ok(new
                    {
                        RspCode = "00",
                        Message = "Confirm Success"
                    });
                }

                return BadRequest(new
                {
                    RspCode = response.VnPayResponseCode,
                    Message = "Fail!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("managed-bookings/{bookingID}")]
        public async Task<IActionResult> GetBooking(Guid bookingID)
        { 
            var rs = await _bookingService.GetBooking(bookingID);
            return Ok(rs);
        }

        private string BookingSend(List<MailBookingModel> mailBookingResponses)
        {
            StringBuilder emailBody = new StringBuilder();

            foreach (var bookingResponse in mailBookingResponses)
            {
                StringBuilder serviceDetails = new StringBuilder();
                foreach (var service in bookingResponse.MailBookingServices)
                {
                    serviceDetails.AppendLine($@"
                   <p style=""font-size: medium; margin: 0;"">
                       <span style=""color: dimgray;"">Dịch vụ:</span>
                       <span style=""font-weight: bold;color: #ea7019;"">{service.ServicePrice:N0}đ</span>
                       <span style=""font-style: italic; font-weight: bold;"">{service.AtStation}</span>
                   </p>");
                }
                emailBody.AppendLine($@"
               <body style=""font-size: 14px; background: #f5f5f5; padding: 20px;"">
                   <h1 style=""text-align: center;"">Xác nhận hoàn thành đặt vé</h1>
                   <table style=""width: 100%; max-width: 600px; margin: 0 auto; background: white; box-shadow: rgba(0, 0, 0, 0.3) 0px 19px 38px, rgba(0, 0, 0, 0.22) 0px 15px 12px; border-collapse: collapse;"">
                       <tr>
                           <td style=""width: 50%; vertical-align: top; border-right: 1px dashed #404040; padding: 20px;"">
                               <div style=""margin-bottom: 20px;"">
                                   <p style=""font-size: large; font-weight: 600;"">Giá vé: <span style=""font-size: x-large; font-weight: 600;color: #ea7019;"">{bookingResponse.Price:N0}đ</span></p>
                                   <p style=""font-size: medium; font-weight: 600;"">Giá dịch vụ:</p>
                               </div>
                               {serviceDetails}
                           </td>
                           <td style=""width: 50%; padding: 20px; text-align: center;"">
                               <p style=""border-top: 1px solid gray; border-bottom: 1px solid gray; padding: 5px 0; font-weight: 700; margin: 20px 0;"">
                                   <span style=""color: #ea7019;"">THE BUS JOURNEY</span>
                               </p>
                               <div>
                                   <h3>{bookingResponse.FullName}</h3>
                                   <h4>Chặng đi: {bookingResponse.FromTo}</h4>
                               </div>
                               <div>
                                   <p>Khởi hành: <span style=""font-size: larger; font-weight: 700"">{bookingResponse.StartTime}</span></p>
                                   <p>Ngày: <span style=""font-size: larger; font-weight: 700"">{bookingResponse.StartDate}</span></p>
                               </div>
                               <p>Vị trí vé: <span style=""font-size: larger; font-weight: 700"">{bookingResponse.SeatCode}</span></p>
                           </td>
                       </tr>
                       <tr>
                           <td colspan=""2"" style=""padding: 20px; text-align: center; background: #F5B642;"">
                               <h1 style=""font-size: 18px;"">Tổng hóa đơn</h1>
                               <h1>{bookingResponse.TotalBill:N0}đ</h1>
                               <div style=""height: 100px; margin: 20px 0;"">
                                   <img src=""{bookingResponse.QrCodeImage}"" alt=""QR code"" style=""height: 100%;"" />
                               </div>
                               <p>Cảm ơn quý khách đã tin tưởng</p>
                           </td>
                       </tr>
                   </table>
               </body>
                ");
            }

            return emailBody.ToString();
        }
    }
}