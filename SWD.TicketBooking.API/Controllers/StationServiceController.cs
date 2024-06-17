using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("station_service")]
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
        [HttpPost("new-service-in-station")]
        public async Task<IActionResult> CreateServiceStation([FromForm] CreateServiceInStationRequest createServiceInStationRequest)
        {
            var serviceStationCreate = _mapper.Map<CreateServiceInStationModel>(createServiceInStationRequest);

            var rs = await _stationServiceService.CreateServiceStation(serviceStationCreate);
            return Ok(rs);
        }
        [HttpPut("service-in-station")]
        public async Task<IActionResult> UpdateServiceStation([FromForm] UpdateServiceInStationRequest updateServiceInStationRequest)
        {
            var serviceStationUpdate = _mapper.Map<UpdateServiceInStationModel>(updateServiceInStationRequest);
            var rs = await _stationServiceService.UpdateServiceStation(serviceStationUpdate);
            return Ok(rs);
        }
    }
}
