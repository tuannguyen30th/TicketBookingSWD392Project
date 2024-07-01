using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Installer;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;
using static SWD.TicketBooking.API.ResponseModels.ServiceFromStationResponse;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("service-management")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceTypeService _serviceTypeService;
        private readonly IServiceService _serviceService;
        private readonly IMapper _mapper;
        public ServiceController(IServiceTypeService serviceTypeService, IServiceService serviceService, IMapper mapper)
        {
            _serviceTypeService = serviceTypeService;
            _serviceService = serviceService;
            _mapper = mapper;
        }

        [HttpGet("managed-services")]
        public async Task<IActionResult> GetAllActiveServices()
        {
            var services = await _serviceService.GetAllActiveServices();
            return Ok(services);
        }

        [HttpGet("managed-services/stations/{stationID}/types/{serviceTypeID}")]
        [Cache(1200)]
        public async Task<IActionResult> ServicesOfTypeFromStations([FromRoute] Guid stationID, [FromRoute] Guid serviceTypeID)
        {
            var serviceTypes = await _serviceTypeService.ServicesOfTypeFromStations(stationID, serviceTypeID);
            var serviceTypeResponses = _mapper.Map<ServiceTypeResponse>(serviceTypes);
            return Ok(serviceTypeResponses);
        }        

        [HttpGet("managed-services/stations/{stationID}")]
        [Cache(1200)]
        public async Task<IActionResult> AllServicesInStations([FromRoute] Guid stationID)
        {
            var serviceTypes = await _serviceTypeService.ServiceTypesFromStation(stationID);
            return Ok(serviceTypes);
        }

        [HttpPost("managed-services")]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest createServiceModel)
        {
            var serviceToUpdate = _mapper.Map<CreateServiceModel>(createServiceModel);

            var service = await _serviceService.CreateService(serviceToUpdate);
            return Ok(service);
        }
        [HttpPut("managed-services/{serviceID}")]
        public async Task<IActionResult> UpdateService([FromBody] UpdateServiceRequest updateServiceModel, [FromRoute] Guid serviceID)
        {
            var serviceToUpdate = _mapper.Map<UpdateServiceModel>(updateServiceModel);

            var updatedService = await _serviceService.UpdateService(serviceToUpdate, serviceID);

            return Ok(updatedService);
        }
        [HttpPut("managed-services/{serviceID}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid serviceID)
        {
            var service = await _serviceService.UpdateStatus(serviceID);
            return Ok(service);
        }
    }
}
