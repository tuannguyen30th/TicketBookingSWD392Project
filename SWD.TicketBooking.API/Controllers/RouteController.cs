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
        private readonly IDistributedCache _cache;
        private readonly IRouteService _routeService;
        private readonly IMapper _mapper;
        private const string FromCityToCityCacheKey = "FromCityToCity";

        public RouteController(ICityService cityService, IDistributedCache cache, IRouteService routeService, IMapper mapper) 
        {
            _cache = cache;
            _cityService = cityService;
            _routeService = routeService;
            _mapper = mapper;
        }
        [HttpGet("managed-routes")]
        [Cache(1200, FromCityToCityCacheKey)]
        public async Task<IActionResult> GetFromCityToCity()
        {
          /*  try
            {
                string cacheKey = "FromCityToCity";
                var cachedData = _cache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedResponse = JsonConvert.DeserializeObject<FromCityToCityRepsonse.CityResponse>(cachedData);
                    return Ok(cachedResponse);
                }
*/
                var dataFromService = await _cityService.GetFromCityToCity();
                var response = _mapper.Map<FromCityToCityRepsonse.CityResponse>(dataFromService);
            /*    DistributedCacheEntryOptions options = new();*/
             /*   options.SetAbsoluteExpiration(new TimeSpan(0, 5, 0));
                _cache.SetString(cacheKey, JsonConvert.SerializeObject(response), options);*/
                return Ok(response);
         /*   }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }*/
        }
        [HttpGet("managed-routes/company-routes/{companyID}")]
        [Cache(1200)]
        public async Task<IActionResult> GetAllRouteFromCompany(Guid companyID)
        {
            var rs = await _routeService.GetAllRouteFromCompany(companyID);
            var response = _mapper.Map<List<GetRouteFromCompanyResponse>>(rs);        
            return Ok(response);
        }
      /*  [AllowAnonymous]
        [HttpGet("managed-routes")]
        public async Task<IActionResult> GetAllRoutes()
        {
            var rs = _mapper.Map<List<RouteResponse>>(await _routeService.GetAllRoutes());
            return Ok(rs);
        }*/

        [AllowAnonymous]
        [HttpPost("managed-routes")]
        [RemoveCache(FromCityToCityCacheKey)]
        public async Task<IActionResult> CreateRoute([FromBody] CreateRouteRequest req)
        {
           /* if (req.FromCityID <= 0 ||  req.ToCityID <= 0 || req.CompanyID <= 0) 
            {
                return BadRequest("Invalid ID");
            }*/
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
        public async Task<IActionResult> UpdateRoute([FromRoute] Guid routeID, [FromBody] UpdateRouteRequest req)
        {
           /* if (routeID <= 0 || req.FromCityID <= 0 || req.ToCityID <= 0)
            {
                return BadRequest("Invalid ID");
            }*/
            var map = _mapper.Map<UpdateRouteModel>(req);
            var rs = await _routeService.UpdateRoute(routeID, map);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }

        [AllowAnonymous]
        [HttpPut("managed-routes/{routeID}/status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid routeID, [FromBody] ChangeStatusRequest req)
        {
         /*   if (routeID <= 0)
            {
                return BadRequest("Invalid ID");
            }*/
            var rs = await _routeService.ChangeStatus(routeID, req.Status);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }
    }
}
