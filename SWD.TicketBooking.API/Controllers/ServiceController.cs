using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services;
using static SWD.TicketBooking.API.Common.ResponseModels.ServiceFromStationResponse;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceTypeService _serviceTypeService;
        private readonly ServiceService _serviceService;
        private readonly IMapper _mapper;
        public ServiceController(ServiceTypeService serviceTypeService, ServiceService serviceService, IMapper mapper)
        {
            _serviceTypeService = serviceTypeService;
            _serviceService = serviceService;
            _mapper = mapper;
        }
       
        [HttpGet("services-of-type-from-station/{stationID}/{serviceTypeID}")]
        public async Task<IActionResult> ServicesOfTypeFromStations([FromRoute] Guid stationID, [FromRoute] Guid serviceTypeID)
        {
            var serviceTypes = await _serviceTypeService.ServicesOfTypeFromStations(stationID, serviceTypeID);
            var serviceTypeResponses = _mapper.Map<ServiceTypeResponse>(serviceTypes);
            return Ok(serviceTypeResponses);
        }

        [HttpPost("new-service")]
        public async Task<IActionResult> CreateService([FromForm] CreateServiceRequest createServiceModel)
        {
            var serviceToUpdate = _mapper.Map<CreateServiceModel>(createServiceModel);

            var service = await _serviceService.CreateService(serviceToUpdate);
            return Ok(service);
        }
        [HttpPut("service")]
        public async Task<IActionResult> UpdateService([FromForm] UpdateServiceRequest updateServiceModel)
        {
            var serviceToUpdate = _mapper.Map<UpdateServiceModel>(updateServiceModel);

            var updatedService = await _serviceService.UpdateService(serviceToUpdate);

            return Ok(updatedService);
        }
        [HttpPut("service-inactive/{serviceID}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid serviceID)
        {
            var service = await _serviceService.UpdateStatus(serviceID);
            return Ok(service);
        }
    }
}
