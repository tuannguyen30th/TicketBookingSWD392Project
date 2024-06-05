using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("station")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly StationService _stationService;
        private readonly IMapper _mapper;
        public StationController(StationService stationService, IMapper mapper)
        {
            _stationService = stationService;
            _mapper = mapper;
        }
        [HttpGet("stations-from-route")]
        public async Task<IActionResult> GetStationsFromRoute(int routeID)
        {
            var stations = await _stationService.GetStationsFromRoute(routeID);
            var stationResponses = _mapper.Map<List<StationFromRouteResponse>>(stations);
            return Ok(stationResponses);
        }
    }
}
