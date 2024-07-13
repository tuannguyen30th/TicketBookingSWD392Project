using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.Dtos.Booking;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("dashboard-management")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService, IMapper mapper)
        {
            _mapper = mapper;
            _dashboardService = dashboardService;
        }

        [HttpGet("managed-dashboards/company/{companyID}")]
        public async Task<IActionResult> GetAllDashboardInfoByCompany([FromRoute] Guid companyID)
        {
            var rs = _mapper.Map<DashboardInfoByCompanyResponse>(await _dashboardService.GetAllDashboardInfoByCompany(companyID));
            return Ok(rs);
        }
        [HttpGet("managed-dashboards/admins")]
        public async Task<IActionResult> DashboardAdmin()
        {
            var rs = await _dashboardService.DashboardAdmin();
            return Ok(rs);
        }
    }
}
