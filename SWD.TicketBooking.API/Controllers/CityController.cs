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
        [HttpPut("city/{cityID}")]
        public async Task<IActionResult> UpdateCompany([FromRoute] Guid cityID, [FromBody] CreateCityRequest req)
        {
 /*           if (cityID == "")
            {
                return BadRequest("Invalid ID");
            }*/
            var map = _mapper.Map<CreateCityModel>(req);
            var rs = await _cityService.UpdateCity(cityID, map);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }

        [AllowAnonymous]
        [HttpPut("city-status/{cityID}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid cityID, [FromBody] ChangeStatusRequest req)
        {
          /*  if (cityID <= 0)
            {
                return BadRequest("Invalid ID");
            }*/
            var rs = await _cityService.ChangeStatus(cityID, req.Status);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }
    }
}
