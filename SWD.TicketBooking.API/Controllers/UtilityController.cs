using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("utility")]
    [ApiController]
    public class UtilityController : Controller
    {
        private readonly UtilityService _utilityService;
        private readonly IMapper _mapper;
        public UtilityController(UtilityService utilityService, IMapper mapper)
        {
            _mapper = mapper;
            _utilityService = utilityService;
        }
        [HttpGet("trip/{utilityID}")]
        public async Task<IActionResult> GetUtilityByTripID([FromRoute] int utilityID) 
        {
               var rs = _mapper.Map<List<UtilityInTripResponse>>(await _utilityService.GetAllUtilityByTripID(utilityID));
           // var rs = _utilityService.GetAllUtilityByTripID(id);
            return Ok(rs);
        }
    }
}
