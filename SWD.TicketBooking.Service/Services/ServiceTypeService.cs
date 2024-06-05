using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStation;

namespace SWD.TicketBooking.Service.Services
{
    public class ServiceTypeService
    {
        private readonly IRepository<ServiceType, int> _serviceTypeRepository;
        private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, int> _serviceRepository;
        private readonly IRepository<Station_Service, int> _stationServiceRepository;
        private readonly IMapper _mapper;
        public ServiceTypeService(IRepository<ServiceType, int> serviceTypeRepository, IRepository<SWD.TicketBooking.Repo.Entities.Service, int> serviceRepository, IRepository<Station_Service, int> stationServiceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _serviceTypeRepository = serviceTypeRepository;
            _stationServiceRepository = stationServiceRepository;
            _mapper = mapper;
        }
        public async Task<List<ServiceTypeModel>> ServiceFromStations(int stationID)
        {
            try
            {
                var stationServices = await _stationServiceRepository
                    .FindByCondition(_ => _.StationID == stationID)
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
                            Price = _.Service.Price
                        }).ToList()
                    }).ToList();

                return serviceTypes;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<ServiceTypeModel> ServicesOfTypeFromStations(int stationID, int serviceTypeID)
        {
            try
            {
                var stationServices = await _stationServiceRepository
                                        .FindByCondition(_ => _.StationID == stationID && _.Service.ServiceTypeID == serviceTypeID)
                                        .Include(_ => _.Service)
                                        .ThenInclude(_ => _.ServiceType)
                                        .ToListAsync();

                var serviceModels = stationServices.Select(_ => new ServiceModel
                                                    {
                                                     ServiceID = _.Service.ServiceID,
                                                     Name = _.Service.Name,
                                                     Price = _.Service.Price
                                                     }).ToList();

                var serviceTypeModel = new ServiceTypeModel
                {
                    ServiceTypeID = serviceTypeID,
                    Name = stationServices.First()?.Service?.ServiceType?.Name,
                    ServiceModels = serviceModels
                };

                return serviceTypeModel;
            }
            catch (Exception ex)
            {
              
                throw new Exception();
            }
        }

    }
}
