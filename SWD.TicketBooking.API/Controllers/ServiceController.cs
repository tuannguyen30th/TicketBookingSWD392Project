using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Services;
using static SWD.TicketBooking.API.Common.ResponseModels.ServiceFromStationResponse;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStation;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceTypeService _serviceTypeService;
        private readonly IMapper _mapper;
        public ServiceController(ServiceTypeService serviceTypeService, IMapper mapper)
        {
            _serviceTypeService = serviceTypeService;
            _mapper = mapper;
        }
        [HttpGet("service-types-from-station")]
        public async Task<IActionResult> ServiceFromStations(int stationID)
        {
            var serviceTypes = await _serviceTypeService.ServiceFromStations(stationID);
            var serviceTypeResponses = _mapper.Map<List<ServiceTypeResponse>>(serviceTypes);
            return Ok(serviceTypeResponses);
        }
        [HttpGet("services-of-type-from-station")]
        public async Task<IActionResult> ServicesOfTypeFromStations(int stationID, int serviceTypeID)
        {
            var serviceTypes = await _serviceTypeService.ServicesOfTypeFromStations(stationID, serviceTypeID);
            var serviceTypeResponses = _mapper.Map<ServiceTypeResponse>(serviceTypes);
            return Ok(serviceTypeResponses);
        }
    }
}
