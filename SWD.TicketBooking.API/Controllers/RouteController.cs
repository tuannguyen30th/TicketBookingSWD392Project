using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SWD.TicketBooking.API.Installer;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("route-management")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IRouteService _routeService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly IResponseCacheService _responseCacheService;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IDistributedCache cache, IResponseCacheService responseCacheService, ILogger<RouteController> logger, ICityService cityService, IRouteService routeService, IMapper mapper) 
        {
            _cityService = cityService;
            _routeService = routeService;
            _mapper = mapper;
            _responseCacheService = responseCacheService;
            _logger = logger;
            _cache = cache; 
        }
        [HttpGet("managed-routes")]
        [Cache(1200)]
        public async Task<IActionResult> GetFromCityToCity()
        {

                var dataFromService = await _cityService.GetFromCityToCity();
                var response = _mapper.Map<FromCityToCityRepsonse.CityResponse>(dataFromService);
                return Ok(response);

        }
        [HttpGet("managed-routes/company-routes/{companyID}")]
        public async Task<IActionResult> GetAllRouteFromCompany(Guid companyID)
        {
            var rs = await _routeService.GetAllRouteFromCompany(companyID);
            var response = _mapper.Map<List<GetRouteFromCompanyResponse>>(rs);        
            return Ok(response);
        }        
        
        [HttpGet("managed-routes/popular")]
        public async Task<IActionResult> GetPopularRoutes()
        {
            var rs = await _routeService.GetPopularRoutes();
            //var response = _mapper.Map<List<GetRouteFromCompanyResponse>>(rs);        
            return Ok(rs);
        }
        /*        [AllowAnonymous]
                [HttpGet("managed-routes")]
                public async Task<IActionResult> GetAllRoutes()
                {
                    var rs = _mapper.Map<List<RouteResponse>>(await _routeService.GetAllRoutes());
                    return Ok(rs);
                }*/

        [AllowAnonymous]
        [HttpPost("managed-routes")]
        public async Task<IActionResult> CreateRoute([FromBody] CreateRouteRequest req)
        {
            var map = _mapper.Map<CreateRouteModel>(req);
            var rs = await _routeService.CreateRoute(map);
            if (rs < 1)
            {
                return BadRequest("Create failed");
            }
            return Ok("Create successfully");
        }

        [AllowAnonymous]
        [HttpPut("managed-routes/{routeID}")]
        public async Task<IActionResult> UpdateRoute([FromRoute] Guid routeID, [FromBody] CreateRouteRequest req)
        {
            /* if (routeID <= 0 || req.FromCityID <= 0 || req.ToCityID <= 0)
             {
                 return BadRequest("Invalid ID");
             }*/
            var map = _mapper.Map<CreateRouteModel>(req);
            var rs = await _routeService.UpdateRoute(routeID, map);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }

    }
}
