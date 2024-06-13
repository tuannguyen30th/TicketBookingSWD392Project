using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.Service.Dtos.Booking;
using SWD.TicketBooking.Service.Services;

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
            var rs = await _bookingService.AddOrUpdateBooking(bookingModel);
            return Ok(rs);
        }
    }
}
