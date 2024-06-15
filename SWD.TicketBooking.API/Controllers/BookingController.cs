/*using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.Service.Dtos.Booking;
using SWD.TicketBooking.Service.Services;
using SWD.TicketBooking.Service.Services.PaymentService;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly BookingService _bookingService;
        public BookingController(BookingService bookingService, IMapper mapper)
        {
            _mapper = mapper;
            _bookingService = bookingService;
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateBooking(BookingModel bookingModel)
        {
            var rs = await _bookingService.AddOrUpdateBooking(bookingModel, HttpContext);
            return Ok(rs);
        }
        [HttpGet("vnpay-ipn")]
        public async Task<IActionResult> VNPayIPN()
        {
            try
            {
                var response = new VNPayResponseDto
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
                    var orderId = response.BookingId.ToString().Split(" ");
                                      
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(new
            {
                RspCode = "00",
                Message = "Confirm Success"
            });
        }
    }
}
*/