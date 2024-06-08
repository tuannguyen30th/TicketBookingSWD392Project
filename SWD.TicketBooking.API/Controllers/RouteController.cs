using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
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
        [HttpGet("all-routes")]
        public async Task<IActionResult> GetAllRoutes()
        {
            var rs = _mapper.Map<List<RouteResponse>>(await _routeService.GetAllRoutes());
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpPost("new-route")]
        public async Task<IActionResult> CreateRoute([FromBody] CreateRouteRequest req)
        {
            if (req.FromCityID <= 0 ||  req.ToCityID <= 0 || req.CompanyID <= 0) 
            {
                return BadRequest("Invalid ID");
            }
            var map = _mapper.Map<CreateRouteModel>(req);
            var rs = await _routeService.CreateRoute(map);
            if (rs < 1)
            {
                return BadRequest("Create failed");
            }
            return Ok("Create successfully");
        }

        [AllowAnonymous]
        [HttpPut("route/{routeID}")]
        public async Task<IActionResult> UpdateRoute([FromRoute] int routeID, [FromBody] UpdateRouteRequest req)
        {
            if (routeID <= 0 || req.FromCityID <= 0 || req.ToCityID <= 0)
            {
                return BadRequest("Invalid ID");
            }
            var map = _mapper.Map<UpdateRouteModel>(req);
            var rs = await _routeService.UpdateRoute(routeID, map);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }

        [AllowAnonymous]
        [HttpPut("route-status/{routeID}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int routeID, [FromBody] ChangeStatusRequest req)
        {
            if (routeID <= 0)
            {
                return BadRequest("Invalid ID");
            }
            var rs = await _routeService.ChangeStatus(routeID, req.Status);
            if (rs < 1)
            {
                return BadRequest("Update failed");
            }
            return Ok("Update successfully");
        }
    }
}
