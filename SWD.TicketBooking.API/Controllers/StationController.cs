using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
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

        [HttpGet("all-stations")]
        public async Task<IActionResult> GetAllStations()
        {
            var stations = await _stationService.GetAllStationActive();
            var rs = _mapper.Map<List<GetStationResponse>>(stations);
            return Ok(rs);
        }

        [HttpGet("station-by-id/{id}")]
        public async Task<IActionResult> GetStationById(int id)
        {
            var station = await _stationService.GetStationById(id);
            var rs = _mapper.Map<GetStationResponse>(station);
            return Ok(rs);
        }

        [HttpPost("new-staion")]
        public async Task<IActionResult> CreateNewStation([FromBody] CreateStationRequest request)
        {
            var map = _mapper.Map<CreateStationModel>(request);
            var rs = await _stationService.CreateStation(map);
            return Ok(rs);
        }
        [HttpPut("station/{id}")]
        public async Task<IActionResult> UpdateStaion([FromRoute] int id, [FromBody] CreateStationRequest req)
        {
            var map = _mapper.Map<CreateStationModel>(req);
            var rs = await _stationService.UpdateStation(id, map);
            return Ok(rs);
        }
    }
}
