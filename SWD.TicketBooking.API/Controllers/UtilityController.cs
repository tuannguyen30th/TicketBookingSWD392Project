using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
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

        [HttpGet("")]
        public async Task<IActionResult> GetAllUtility()
        {
            var utilityList = await _utilityService.GetAllUtility();
            var rs = _mapper.Map<List<UtilityReponse>>(utilityList);
            return Ok(rs);
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewUtility([FromBody] CreateNewUtilityRequest req)
        {
            var map = _mapper.Map<CreateUtilityModel>(req);
            var rs = await _utilityService.CreateNewUtility(map);
            if (rs < 1)
            {
                return BadRequest("Create failed!");
            }
            return Ok(rs);
        }
    }
}
