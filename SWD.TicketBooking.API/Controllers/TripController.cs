using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services;
using static SWD.TicketBooking.Service.Dtos.CreateTripModel;

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
        [HttpGet("trip-picture-detail/{tripId}")]
        public async Task<IActionResult> GetTripPictureDetail(int tripId)
        {
            var rs = _mapper.Map<List<GetPictureResponse>>(await _tripService.GetPictureOfTrip(tripId));
            return Ok(rs);
        }
        [HttpGet("list-trip-fromCity-toCity/{fromCity}/{toCity}/{startTime}/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> SearchTrip(int fromCity, int toCity, DateTime startTime, int pageNumber = 1, int pageSize = 10)
        {
            var rs = await _tripService.SearchTrip(fromCity, toCity, startTime, pageNumber, pageSize);
            var rsMapper = _mapper.Map<List<SearchTripResponse>>(rs.Items);

            var paginatedResult = new
            {
                Data = rsMapper,
                TotalPages = rs.TotalCount,
            };

            return Ok(paginatedResult);
        }
        [HttpPost("new-trip")]
        public async Task<IActionResult> CreateTrip([FromForm] CreateTripModel createTripRequest)
        {
            //var createTrip = _mapper.Map<CreateTripModel>(createTripRequest);

            var updatedService = await _tripService.CreateTrip(createTripRequest);

            return Ok(updatedService);
        }
        [HttpPut("trip/{tripID}")]
        public async Task<IActionResult> ChangeStatusTrip([FromRoute] int tripID)
        {

            var updatedService = await _tripService.ChangeStatusTrip(tripID);
            return Ok(updatedService);
        }
    }
}
