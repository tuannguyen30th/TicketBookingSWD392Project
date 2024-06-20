
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;
using static SWD.TicketBooking.Service.Dtos.CreateTripModel;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("trip-management")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IMapper _mapper;

        public TripController(ITripService tripService, IMapper mapper)
        {
            _tripService = tripService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("manage-trips/populars")]
        public async Task<IActionResult> GetPopularTrips()
        {
            var rs = _mapper.Map<List<PopularTripResponse>>(await _tripService.GetPopularTrips());
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpGet("manage-trips/{tripId}/pictures")]
        public async Task<IActionResult> GetTripPictureDetail(Guid tripId)
        {
            //   var rs = _mapper.Map<List<GetPictureResponse>>(await _tripService.GetPictureOfTrip(tripId));
            var rs = new List<string>();
            rs = await _tripService.GetPictureOfTrip(tripId);
            return Ok(rs);
        }
        [HttpGet("managed-trips/from-city/{fromCity}/to-city/{toCity}/start-time/{startTime}/page-number/{pageNumber}/page-size/{pageSize}")]
        public async Task<IActionResult> SearchTrip(Guid fromCity, Guid toCity, DateTime startTime, int pageNumber = 1, int pageSize = 10)
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
        [HttpPost("managed-trips")]
        public async Task<IActionResult> CreateTrip([FromForm] CreateTripModel createTripRequest)
        {
            //var createTrip = _mapper.Map<CreateTripModel>(createTripRequest);

            var updatedService = await _tripService.CreateTrip(createTripRequest);

            return Ok(updatedService);
        }
        [HttpPut("managed-trips/{tripID}")]
        public async Task<IActionResult> ChangeStatusTrip([FromRoute] Guid tripID)
        {

            var updatedService = await _tripService.ChangeStatusTrip(tripID);
            return Ok(updatedService);
        }
        [HttpGet("managed-trips/{tripID}/booked-seats")]
        public async Task<IActionResult> GetSeatBookedFromTrip(Guid tripID)
        {
            var rs = await _tripService.GetSeatBookedFromTrip(tripID);
            return Ok(rs);
        }
        [HttpGet("managed-trips/{tripID}/utilities")]
        public async Task<IActionResult> GetUtilityByTripID([FromRoute] Guid tripID)
        {
            var rs = _mapper.Map<List<UtilityInTripResponse>>(await _tripService.GetAllUtilityByTripID(tripID));
            return Ok(rs);
        }

    }
}

