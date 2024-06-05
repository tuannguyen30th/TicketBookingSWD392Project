using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("route")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly RouteService _routeService;
        private readonly IMapper _mapper;

        public RouteController(RouteService routeService, IMapper mapper) 
        {
            _routeService = routeService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("get-all-routes")]
        public async Task<IActionResult> GetAllRoutes()
        {
            var rs = _mapper.Map<List<RouteResponse>>(await _routeService.GetAllRoutes());
            return Ok(rs);
        }
    }
}
