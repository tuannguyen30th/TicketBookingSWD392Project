using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("ticketDetail")]
    [ApiController]
    public class TicketDetailController : ControllerBase
    {
        private readonly TicketDetailService _ticketDetailService;
        private readonly IMapper _mapper;

        public TicketDetailController(TicketDetailService ticketDetailService, IMapper mapper)
        {
            _ticketDetailService = ticketDetailService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("ticket-details-by-user/{userID}")]
        public async Task<IActionResult> GetTicketDetailByUser([FromRoute] int userID)
        {
            try
            {
                var rs = _mapper.Map<List<GetTicketDetailByUserResponse>>(await _ticketDetailService.GetTicketDetailByUser(userID));
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
