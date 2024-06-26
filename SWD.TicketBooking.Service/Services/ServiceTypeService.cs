﻿using AutoMapper;
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
        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        public ServiceTypeService(IUnitOfWork unitOfWork,IFirebaseService firebaseService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }
        public async Task<List<ServiceTypeModel>> ServiceTypesFromStation(Guid stationID)
        {
            try
            {
                var stationServices = await _unitOfWork.Station_ServiceRepository
                                                       .FindByCondition(_ => _.StationID == stationID 
                                                                          && _.Service.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
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
                            ServiceID = (Guid)_.ServiceID,
                            Name = _.Service.Name,
                            Price = (double)_.Price,
                            ImageUrl = _.ImageUrl
                        }).ToList()
                    }).ToList();

                return serviceTypes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ServiceTypeModel> ServicesOfTypeFromStations(Guid stationID, Guid serviceTypeID)
        {
            try
            {
                var stationServices = await _unitOfWork.Station_ServiceRepository
                                                       .FindByCondition(_ => _.StationID == stationID && _.Service.ServiceTypeID == serviceTypeID && _.Service.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                       .Include(_ => _.Service)
                                                       .ThenInclude(_ => _.ServiceType)
                                                       .ToListAsync();

                var serviceModelsTasks = stationServices.Select(async _ => new ServiceModel
                {
                    ServiceID = _.Service.ServiceID,
                    Name = _.Service.Name,
                    Price = (double)_.Price,
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
