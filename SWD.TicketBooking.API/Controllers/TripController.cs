
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SWD.TicketBooking.API.Installer;
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
        private readonly IDistributedCache _cache;
        private readonly IResponseCacheService _responseCacheService;
        private readonly ILogger<TripController> _logger;

        public TripController(ILogger<TripController> logger, IResponseCacheService responseCacheService, IDistributedCache cache, ITripService tripService, IMapper mapper)
        {
            _cache = cache;
            _tripService = tripService;
            _mapper = mapper;
            _logger = logger;
            _responseCacheService = responseCacheService;
        }

        [AllowAnonymous]
        [HttpGet("manage-trips/populars")]
        public async Task<IActionResult> GetPopularTrips()
        {
            var rs = _mapper.Map<List<PopularTripResponse>>(await _tripService.GetPopularTrips());
            return Ok(rs);
        }
        [HttpGet("manage-trips/{tripID}/seats")]
        public async Task<IActionResult> GetAllSeatsFromTrip(Guid tripID)
        {
            var rs = await _tripService.GetAllSeatsFromTrip(tripID);
            return Ok(rs);
        }
        [AllowAnonymous]
        [HttpGet("manage-trips/{tripId}/pictures")]
        public async Task<IActionResult> GetTripPictureDetail(Guid tripId)
        {
            var rs = new List<string>();
            rs = await _tripService.GetPictureOfTrip(tripId);
            return Ok(rs);
        }
        [AllowAnonymous]
        [HttpGet("manage-trips/{companyID}")]
        public async Task<IActionResult> GetTripsFromCompany(Guid companyID)
        {
            var rs = await _tripService.GetTripsFromCompany(companyID);
            return Ok(rs);
        }
        [AllowAnonymous]
        [HttpGet("manage-trips/{tripID}/details")]
        public async Task<IActionResult> TripDetails(Guid tripID)
        {
            var rs = await _tripService.TripDetails(tripID);
            return Ok(rs);
        }
        [HttpGet("managed-trips/from-city/{fromCity}/to-city/{toCity}/start-time/{startTime}/page-number/{pageNumber}/page-size/{pageSize}")]
        [Cache(1200)]
        public async Task<IActionResult> SearchTrip(
             [FromRoute] Guid fromCity,
             [FromRoute] Guid toCity,
             [FromRoute] DateTime startTime,
             [FromRoute] int pageNumber = 1,
             [FromRoute] int pageSize = 10,
             [FromQuery] string[]? seatAvailability = null,
             [FromQuery] string? sortOption = null,
             [FromQuery] Guid[]? sortCompany = null)
        {

            var dataFromService = await _tripService.SearchTrip(
                                                     fromCity,
                                                     toCity,
                                                     startTime,
                                                     pageNumber,
                                                     pageSize,
                                                     seatAvailability,
                                                     sortOption,
                                                     sortCompany);
            var response = _mapper.Map<PagedResultResponse<SearchTripResponse>>(dataFromService);
            return Ok(response);
        }


        [HttpPost("managed-trips")]
        public async Task<IActionResult> CreateTrip([FromForm] CreateTripModel createTripRequest)
        {
            var updatedService = await _tripService.CreateTrip(createTripRequest);
            await _responseCacheService.RemoveCacheResponseAsync("/trip-management/managed-trips/from-city");
            return Ok(updatedService);
        }        
        
        [HttpPut("managed-trips/trip/{tripID}")]
        public async Task<IActionResult> UpdateTrip([FromBody] UpdateTripModel updateTripRequest, [FromRoute] Guid tripID)
        {
            var updatedService = await _tripService.UpdateTrip(updateTripRequest, tripID);
            return Ok(updatedService);
        }

        [HttpPut("managed-trips/{tripID}")]
        public async Task<IActionResult> ChangeStatusTrip([FromRoute] Guid tripID)
        {

            var updatedService = await _tripService.ChangeStatusTrip(tripID);
            await _responseCacheService.RemoveCacheResponseAsync("/trip-management/managed-trips/from-city");
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

        [HttpGet("managed-trips/ticket-type")]
        public async Task<IActionResult> GetAllTicketType()
        {
            var ticketType = await _tripService.GetAllTicketType();
            var rs = _mapper.Map<List<TicketTypeResponse>>(ticketType);
            return Ok(rs);
        }

        [HttpGet("managed-trips/staff/{staffID}/start-time/{startTime}")]
        public async Task<IActionResult> SearchTrip(Guid staffID, DateTime startTime)
        {

            var dataFromService = await _tripService.GetAllTripsByStaffAndDate(staffID, startTime);
            var response = _mapper.Map<List<SearchTripResponse>>(dataFromService);
            return Ok(response);
        }
    }
}

