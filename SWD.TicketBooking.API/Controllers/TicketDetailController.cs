using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("ticketDetail")]
    [ApiController]
    public class TicketDetailController : ControllerBase
    {
        private readonly ITicketDetailService _ticketDetailService;
        private readonly IMapper _mapper;

        public TicketDetailController(ITicketDetailService ticketDetailService, IMapper mapper)
        {
            _ticketDetailService = ticketDetailService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("ticket-details/{ticketDetailID}")]
        public async Task<IActionResult> GetDetailTicketDetailByTicketDetail([FromRoute] Guid ticketDetailID)
        {
            try
            {
                var rs = _mapper.Map<GetDetailTicketDetailByTicketDetailResponse>(await _ticketDetailService.GetDetailTicketDetailByTicketDetail(ticketDetailID));
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("ticket-details-by-user/{userID}")]
        public async Task<IActionResult> GetTicketDetailByUser([FromRoute] Guid userID)
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

        [HttpGet("ticket-detail-by-QRCode/{qrCode}/{email}")]
        public async Task<IActionResult> SearchTicket([FromRoute] string qrCode, [FromRoute] string email)
        {
            try
            {
                var rs = _mapper.Map<SearchTicketResponse>(await _ticketDetailService.SearchTicket(qrCode, email));
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

    }
}
