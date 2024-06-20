using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.Entities;
using AutoMapper;
using SWD.TicketBooking.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;
using SWD.TicketBooking.Service.IServices;


namespace SWD.TicketBooking.Service.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> _serviceRepository;
        private readonly IRepository<Station_Service, Guid> _stationServiceRepository;
        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        public ServiceService(IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> serviceRepository, IRepository<Station_Service, Guid> stationServiceRepository, IFirebaseService firebaseService, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _stationServiceRepository = stationServiceRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }

        public async Task<int> CreateService(CreateServiceModel createServiceModel)
        {
            try
            {
                var checkExistedName = await _serviceRepository.GetAll().Where(_ => _.ServiceTypeID == createServiceModel.ServiceTypeID && _.Name.ToLower().Trim().Equals(createServiceModel.Name.ToLower())).FirstOrDefaultAsync();
                if (checkExistedName != null)
                {
                    throw new BadRequestException("Service name existed");
                }

                var service = new SWD.TicketBooking.Repo.Entities.Service
                {
                    ServiceID = Guid.NewGuid(),
                    Name = createServiceModel.Name,
                    ServiceTypeID = createServiceModel.ServiceTypeID,
                    Status = SD.ACTIVE
                };
                await _serviceRepository.AddAsync(service);
                var rs = await _serviceRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
      
        public async Task<int> UpdateService(UpdateServiceModel updateServiceModel, Guid serviceID)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(serviceID);
                if (service == null)
                {
                    throw new NotFoundException("Service not found.");
                }

                var checkExistedName = await _serviceRepository.GetAll()
                                      .Where(_ => _.ServiceTypeID == updateServiceModel.ServiceTypeID
                                       && _.Name.ToLower().Trim().Equals(updateServiceModel.Name.ToLower())
                                       && _.ServiceID != serviceID)
                                      .FirstOrDefaultAsync();

                if (checkExistedName != null)
                {
                    throw new BadRequestException("Service name existed");
                }

                service.Name = updateServiceModel.Name;
                service.ServiceTypeID = updateServiceModel.ServiceTypeID;            
                _serviceRepository.Update(service);
                var rs = await _serviceRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating service", ex);
            }
        }

        public async Task<bool> UpdateStatus(Guid serviceID)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(serviceID);
                if (service == null)
                {
                    throw new NotFoundException("Service not found.");
                }
                service.Status = SD.INACTIVE;
                _serviceRepository.Update(service);
                var rs = await _serviceRepository.Commit();
                if (rs > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
