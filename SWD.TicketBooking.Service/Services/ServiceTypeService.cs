using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Services.FirebaseService;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;

namespace SWD.TicketBooking.Service.Services
{
    public class ServiceTypeService
    {
        private readonly IRepository<ServiceType, Guid> _serviceTypeRepository;
        private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> _serviceRepository;
        public readonly IFirebaseService _firebaseService;
        private readonly IRepository<Station_Service, Guid> _stationServiceRepository;
        private readonly IRepository<Station_Route, Guid> _stationRouteRepository;
        private readonly IMapper _mapper;
        public ServiceTypeService(IRepository<Station_Route, Guid> stationRouteRepository, IRepository<ServiceType, Guid> serviceTypeRepository, IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> serviceRepository, IRepository<Station_Service, Guid> stationServiceRepository, IFirebaseService firebaseService, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _stationRouteRepository = stationRouteRepository;
            _stationServiceRepository = stationServiceRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }    
     /*   public async Task<List<ServiceTypeModel>> ServiceTypesFromStation(Guid stationID)
        {
            try
            {
                var stationServices = await _stationServiceRepository
                    .FindByCondition(_ => _.StationID == stationID && _.Service.Status.Trim().Equals(SD.ACTIVE))
                    .Include(_ => _.Service)
                    .ThenInclude(_ => _.ServiceType)
                    .ToListAsync();

                var serviceTypes = stationServices
                    .GroupBy(_ => _.Service.ServiceType)
                    .Select(_ => new ServiceTypeModel
                    {
                        ServiceTypeID = _.Key.ServiceTypeID,
                        Name = _.Key.Name,
                        ServiceModels = _.Select(_ => new ServiceModel
                        {
                            ServiceID = _.Service.ServiceID,
                            Name = _.Service.Name,
                        }).ToList()
                    }).ToList();

                return serviceTypes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }*/
        public async Task<ServiceTypeModel> ServicesOfTypeFromStations(Guid stationID, Guid serviceTypeID)
        {
            try
            {
                var stationServices = await _stationServiceRepository
                                        .FindByCondition(_ => _.StationID == stationID && _.Service.ServiceTypeID == serviceTypeID && _.Service.Status.Trim().Equals(SD.ACTIVE))
                                        .Include(_ => _.Service)
                                        .ThenInclude(_ => _.ServiceType)
                                        .ToListAsync();

                var serviceModelsTasks = stationServices.Select(async _ => new ServiceModel
                {
                    ServiceID = _.Service.ServiceID,
                    Name = _.Service.Name,
                    Price = _.Price,
                    ImageUrl = _.ImageUrl
                }).ToList();

                var serviceModels = await Task.WhenAll(serviceModelsTasks);

                var serviceTypeModel = new ServiceTypeModel
                {
                    ServiceTypeID = serviceTypeID,
                    Name = stationServices.First()?.Service?.ServiceType?.Name,
                    ServiceModels = serviceModels.ToList()
                };

                return serviceTypeModel;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

    }
}
