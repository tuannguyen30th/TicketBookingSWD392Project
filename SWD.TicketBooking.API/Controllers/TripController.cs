using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("trip")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly TripService _tripService;
        private readonly IMapper _mapper;

        public TripController(TripService tripService, IMapper mapper)
        {
            _tripService = tripService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("popular-trips")]
        public async Task<IActionResult> GetPopularTrips()
        {
            var rs = _mapper.Map<List<PopularTripResponse>>(await _tripService.GetPopularTrips());
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpGet("trip-detail/{id}")]
        public async Task<IActionResult> GetTripPictureDetail(int id)
        {
            var rs = _mapper.Map<List<GetPictureResponse>>(await _tripService.GetPictureOfTrip(id));
            return Ok(rs);
        }
    }
}
