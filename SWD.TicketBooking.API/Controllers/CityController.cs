using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("city")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityService _cityService;
        private readonly IMapper _mapper;

        public CityController(CityService cityService, IMapper mapper)
        {
            _cityService = cityService;
            _mapper = mapper;
        }
        [HttpGet("from-cities-to-cities")]
        public async Task<IActionResult> GetFromCityToCity()
        {
            try
            {
                var rs = _mapper.Map<FromCityToCityRepsonse.CityResponse>(await _cityService.GetFromCityToCity());
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        [AllowAnonymous]
        [HttpPost("new-city")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCityRequest req)
        {
            var map = _mapper.Map<CreateCityModel>(req);
            var rs = await _cityService.CreateCity(map);
            if (rs < 1)
            {
                return BadRequest("Create failed");
            }
            return Ok("Create successfully");
        }

        [AllowAnonymous]
        [HttpPut("city/{id}")]
        public async Task<IActionResult> UpdateCompany([FromRoute] int id, [FromBody] CreateCityRequest req)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }
            var map = _mapper.Map<CreateCityModel>(req);
            var rs = await _cityService.UpdateCity(id, map);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }

        [AllowAnonymous]
        [HttpPut("city-status/{id}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] ChangeStatusRequest req)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }
            var rs = await _cityService.ChangeStatus(id, req.Status);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }
    }
}
