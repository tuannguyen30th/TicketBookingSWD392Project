using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.BackendService;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;
using System.Text.Json;

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
       

        [AllowAnonymous]
        [HttpPost("managed-cities")]
        
        public async Task<ActionOutcome> CreateCompany([FromBody] CreateCityRequest req)
        {
            var map = _mapper.Map<CreateCityModel>(req);
            return await _cityService.CreateCity(map);
        }

        [AllowAnonymous]
        [HttpPut("managed-cities/{cityID}")]
        public async Task<ActionOutcome> UpdateCompany([FromRoute] Guid cityID, [FromBody] CreateCityRequest req)
        {
            var map = _mapper.Map<CreateCityModel>(req);
            return await _cityService.UpdateCity(cityID, map);
        }

        [AllowAnonymous]
        [HttpPut("managed-cities/{cityID}/status")]
        public async Task<ActionOutcome> ChangeStatus([FromRoute] Guid cityID, [FromBody] ChangeStatusRequest req)
        {
            return await _cityService.ChangeStatus(cityID, req.Status);
        }
    }
}
