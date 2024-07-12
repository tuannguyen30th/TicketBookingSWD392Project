using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SWD.TicketBooking.API.Installer;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("station-management")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationService _stationService;
        private readonly IMapper _mapper;
        private readonly ILogger<CityController> _logger;
        private readonly IDistributedCache _cache;
        private readonly IResponseCacheService _responseCacheService;
        public StationController(ILogger<CityController> logger, IDistributedCache cache, IResponseCacheService responseCacheService, IStationService stationService, IMapper mapper)
        {
            _stationService = stationService;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _responseCacheService = responseCacheService;
        }

        [HttpGet("managed-stations/company/{companyID}")]
        public async Task<IActionResult> GetAllStationsByCompanyID(Guid companyID)
        {
            var stations = await _stationService.GetAllStationsByCompanyID(companyID);
            var stationResponses = _mapper.Map<List<GetStationByCompanyResponse>>(stations);
            return Ok(stationResponses);
        }

        [HttpGet("managed-stations/city/{cityID}")]
        ///*[Cache(1200)]*/
        public async Task<IActionResult> GetAllStationsByCityID(Guid cityID)
        {
            var stations = await _stationService.GetStationsByCityId(cityID);
            var stationResponses = _mapper.Map<List<GetStationResponse>>(stations);
            return Ok(stationResponses);
        }

        [HttpGet("managed-stations/routes/{routeID}/companyID/{companyID}")]
        public async Task<IActionResult> GetStationsFromTrip(Guid routeID, Guid companyID)
        {
            var stations = await _stationService.GetStationsFromTrip(routeID, companyID);
            var stationResponses = _mapper.Map<List<StationFromRouteResponse>>(stations);
            return Ok(stationResponses);
        }

        //[HttpGet("managed-stations/trips/{tripID}")]
        ////[Cache(1200)]
        //public async Task<IActionResult> GetStationsInTrip(Guid tripID)
        //{
        //    var stations = await _stationService.GetAllStationInRoute(tripID);
        //    var stationResponses = _mapper.Map<List<StationFromRouteResponse>>(stations);
        //    return Ok(stationResponses);
        //}

        [HttpGet("managed-stations")]
   
        public async Task<IActionResult> GetAllStations()
        {
            var stations = await _stationService.GetAllStationActive();
            var rs = _mapper.Map<List<GetStationResponse>>(stations);
            return Ok(rs);
        }

        [HttpGet("managed-stations/{stationID}")]
        public async Task<IActionResult> GetStationById(Guid stationID)
        {
            var station = await _stationService.GetStationById(stationID);
            var rs = _mapper.Map<GetStationByCompanyResponse>(station);
            return Ok(rs);
        }

        [HttpPost("managed-stations")]
        public async Task<IActionResult> CreateNewStation([FromBody] CreateStationRequest request)
        {
            var map = _mapper.Map<CreateStationModel>(request);
            var rs = await _stationService.CreateStation(map);
            return Ok(rs);
        }

        [HttpPost("managed-stations/company-registation/company/{companyID}")]
        public async Task<IActionResult> CompanyRegisStation([FromRoute] Guid companyID, [FromForm] List<Guid> stationID)
        {
            var rs = await _stationService.CompanyRegisStation(companyID, stationID);
            return Ok(rs);
        }

        [HttpPut("managed-stations/{stationID}")]
        public async Task<IActionResult> UpdateStation([FromRoute] Guid stationID, [FromBody] UpdateStationRequest req)
        {
            var map = _mapper.Map<UpdateStationModel>(req);
            var rs = await _stationService.UpdateStation(stationID, map);
            return Ok(rs);
        }
    }
}
