using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("status-management")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }
        [AllowAnonymous]
        [HttpPut("")]
        public async Task<IActionResult> ChangeStatus(string entity, Guid Id)
        {
            var rs = await _statusService.ChangeStatus(entity, Id);
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpPut("ticket-detail/station/service")]
        public async Task<IActionResult> ChangeStatusServiceInTicketDetail(Guid stationId, Guid ticketDetailId, Guid serviceId)
        {
            var rs = await _statusService.UpdateStatusServiceInTicket( stationId, ticketDetailId, serviceId);
            return Ok(rs);
        }
    }
}
