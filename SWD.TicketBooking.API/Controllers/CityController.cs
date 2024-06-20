using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("city-management")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public CityController(ICityService cityService, IMapper mapper, IDistributedCache cache)
        {
            _cityService = cityService;
            _cache = cache;
            _mapper = mapper;
        }

        [HttpGet("managed-cities")]
        public async Task<IActionResult> GetAllCities()
        {
            string cacheKey = "GetAllCities";
            var cachedData = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                var cachedResponse = JsonConvert.DeserializeObject<List<CitiesResponse>>(cachedData);
                return Ok(cachedResponse);
            }

            var dataFromService = await _cityService.GetAllCities();
            var response = _mapper.Map<List<CitiesResponse>>(dataFromService);
            DistributedCacheEntryOptions options = new();
            options.SetAbsoluteExpiration(new TimeSpan(0, 5, 0));
            _cache.SetString(cacheKey, JsonConvert.SerializeObject(response), options);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("managed-cities")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCityRequest req)
        {
            var map = _mapper.Map<CreateCityModel>(req);
            var rs = await _cityService.CreateCity(map);
            if (rs == null)
            {
                return BadRequest("Create failed");
            }
            string cacheKey = "GetAllCities";
            await _cache.RemoveAsync(cacheKey);
            return Ok("Create successfully");
        }

        [AllowAnonymous]
        [HttpPut("managed-cities/{cityID}")]
        public async Task<IActionResult> UpdateCompany([FromRoute] Guid cityID, [FromBody] CreateCityRequest req)
        {
            var map = _mapper.Map<CreateCityModel>(req);
            var rs = await _cityService.UpdateCity(cityID, map);
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpPut("managed-cities/{cityID}/status")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid cityID, [FromBody] ChangeStatusRequest req)
        {
            return Ok(await _cityService.ChangeStatus(cityID, req.Status));
        }
    }
}