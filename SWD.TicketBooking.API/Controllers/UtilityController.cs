using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("utility-management")]
    [ApiController]
    public class UtilityController : Controller
    {
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;
        public UtilityController(IUtilityService utilityService, IMapper mapper)
        {
            _mapper = mapper;
            _utilityService = utilityService;
        }
      /*  [HttpGet("managedtrip/{tripID}")]
        public async Task<IActionResult> GetUtilityByTripID([FromRoute] Guid tripID) 
        {
               var rs = _mapper.Map<List<UtilityInTripResponse>>(await _utilityService.GetAllUtilityByTripID(tripID));
           // var rs = _utilityService.GetAllUtilityByTripID(id);
            return Ok(rs);
        }*/
    }
}
