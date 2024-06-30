using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.Booking;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("ticket-detail-management")]
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
        [HttpGet("managed-ticket-details/{ticketDetailID}")]
        public async Task<IActionResult> GetDetailOfTicketByID([FromRoute] Guid ticketDetailID)
        {
            try
            {
                var rs = _mapper.Map<GetDetailOfTicketByIDResponse>(await _ticketDetailService.GetDetailOfTicketByID(ticketDetailID));
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("managed-ticket-details/customers/{customerID}")]
        public async Task<IActionResult> GetTicketDetailByUser([FromRoute] Guid customerID)
        {
            try
            {
                var rs = _mapper.Map<List<GetTicketDetailByUserResponse>>(await _ticketDetailService.GetTicketDetailByUser(customerID));
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        [HttpPut("managed-ticket-details/cancel-tickets/{ticketDetailID}")]
        public async Task<ActionOutcome> CancelTicket(Guid ticketDetailID)
        {
            /*  var map = _mapper.Map<BookingModel>(bookingRequest);*/
            var rs = await _ticketDetailService.CancelTicket(ticketDetailID);
            return rs;
        }

        [HttpPut("managed-ticket-details/ticket-verification/{qrCode}")]
        public async Task<IActionResult> VerifyTicketDetail(string qrCode)
        {
            /*  var map = _mapper.Map<BookingModel>(bookingRequest);*/
            var rs = await _ticketDetailService.VerifyTicketDetail(qrCode);
            return Ok(rs);
        }

        [HttpGet("managed-ticket-details/qrCodes/{qrCode}/emails/{email}")]
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

        [HttpGet("managed-ticket-details/qrCodes/{qrCode}")]
        public async Task<IActionResult> GetTicketDetailInMobile([FromRoute] string qrCode)
        {
            try
            {
                var rs = _mapper.Map<GetTicketDetailInMobileResponse>(await _ticketDetailService.GetTicketDetailInMobile(qrCode));
          //      var result = _ticketDetailService.GetTicketDetailInMobile(qrCode);
                return Ok(rs);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

    }
}
