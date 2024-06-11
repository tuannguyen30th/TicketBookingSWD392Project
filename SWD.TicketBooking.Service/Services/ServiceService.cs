using SWD.TicketBooking.Repo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.TicketBooking.Repo.Entities;
using AutoMapper;
using SWD.TicketBooking.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Service.Services.FirebaseService;
using Microsoft.IdentityModel.Tokens;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;


namespace SWD.TicketBooking.Service.Services
{
    public class ServiceService
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
      
        public async Task<int> UpdateService(UpdateServiceModel updateServiceModel)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(updateServiceModel.ServiceID);
                if (service == null)
                {
                    throw new NotFoundException("Service not found.");
                }

                var checkExistedName = await _serviceRepository.GetAll()
                                      .Where(_ => _.ServiceTypeID == updateServiceModel.ServiceTypeID
                                       && _.Name.ToLower().Trim().Equals(updateServiceModel.Name.ToLower())
                                       && _.ServiceID != updateServiceModel.ServiceID)
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
