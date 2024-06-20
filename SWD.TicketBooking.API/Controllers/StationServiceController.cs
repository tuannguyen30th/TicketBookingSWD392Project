using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("station-service-management")]
    [ApiController]
    public class StationServiceController : ControllerBase
    {
        private readonly IStation_ServiceService _stationServiceService;
        private readonly IMapper _mapper;

        public StationServiceController(IStation_ServiceService stationServiceService, IMapper mapper)
        {
            _stationServiceService = stationServiceService;
            _mapper = mapper;
        }
        [HttpPost("managed-station-services")]
        public async Task<IActionResult> CreateServiceStation([FromForm] CreateServiceInStationRequest createServiceInStationRequest)
        {
            var serviceStationCreate = _mapper.Map<CreateServiceInStationModel>(createServiceInStationRequest);

            var rs = await _stationServiceService.CreateServiceStation(serviceStationCreate);
            return Ok(rs);
        }
        [HttpPut("managed-station-services/{stationServiceID}")]
        public async Task<IActionResult> UpdateServiceStation([FromForm] UpdateServiceInStationRequest updateServiceInStationRequest, [FromRoute] Guid stationServiceID)
        {
            var serviceStationUpdate = _mapper.Map<UpdateServiceInStationModel>(updateServiceInStationRequest);
            var rs = await _stationServiceService.UpdateServiceStation(serviceStationUpdate, stationServiceID);
            return Ok(rs);
        }
    }
}
