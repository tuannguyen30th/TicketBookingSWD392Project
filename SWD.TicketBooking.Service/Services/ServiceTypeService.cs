using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;

namespace SWD.TicketBooking.Service.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRepository<ServiceType, Guid> _unitOfWork.ServiceTypeRepository;
        //private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> _unitOfWork.ServiceRepository;
        public readonly IFirebaseService _firebaseService;
        //private readonly IRepository<Station_Service, Guid> _unitOfWork.Station_ServiceRepository;
        //private readonly IRepository<Station_Route, Guid> _unitOfWork.Station_RouteRepository;
        private readonly IMapper _mapper;
        public ServiceTypeService(IUnitOfWork unitOfWork, IRepository<Station_Route, Guid> stationRouteRepository, IRepository<ServiceType, Guid> serviceTypeRepository, IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> serviceRepository, IRepository<Station_Service, Guid> stationServiceRepository, IFirebaseService firebaseService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_unitOfWork.ServiceRepository = serviceRepository;
            //_unitOfWork.ServiceTypeRepository = serviceTypeRepository;
            //_unitOfWork.Station_RouteRepository = stationRouteRepository;
            //_unitOfWork.Station_ServiceRepository = stationServiceRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }    
     /*   public async Task<List<ServiceTypeModel>> ServiceTypesFromStation(Guid stationID)
        {
            try
            {
                var stationServices = await _unitOfWork.Station_ServiceRepository
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
                var stationServices = await _unitOfWork.Station_ServiceRepository
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
