using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.ResponseModels;
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
    }
}
