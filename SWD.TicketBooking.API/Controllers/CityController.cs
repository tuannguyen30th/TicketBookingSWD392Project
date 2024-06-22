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

namespace SWD.TicketBooking.API.Controllers
{
    [Route("city-management")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private const string GetAllCitiesCacheKey = "GetAllCities";
        public CityController(ICityService cityService, IMapper mapper, IDistributedCache cache)
        {
            _cityService = cityService;
            _cache = cache;
            _mapper = mapper;
        }

        [HttpGet("managed-cities")]
        [Cache(600, GetAllCitiesCacheKey)]
        public async Task<IActionResult> GetAllCities()
        {
            var dataFromService = await _cityService.GetAllCities();
            var response = _mapper.Map<List<CitiesResponse>>(dataFromService);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("managed-cities")]
        [RemoveCache(GetAllCitiesCacheKey)]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCityRequest req)
        {
            var map = _mapper.Map<CreateCityModel>(req);
            var rs = await _cityService.CreateCity(map);
            if (rs == null)
            {
                return BadRequest("Create failed");
            }
         /*   string cacheKey = "GetAllCities";
            await _cache.RemoveAsync(cacheKey);*/
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