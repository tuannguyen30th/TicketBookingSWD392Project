using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Services.FirebaseService;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class Station_ServiceService
    {
        private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> _serviceRepository;
        private readonly IRepository<Station_Service, Guid> _stationServiceRepository;
        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        public Station_ServiceService(IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> serviceRepository, IRepository<Station_Service, Guid> stationServiceRepository, IFirebaseService firebaseService, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _stationServiceRepository = stationServiceRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }
        public async Task<bool> CreateServiceStation(CreateServiceInStationModel createServiceInStationModel)
        {
            try
            {
                var checkExisted = await _stationServiceRepository.FindByCondition(_ => _.ServiceID == createServiceInStationModel.ServiceID && _.StationID == createServiceInStationModel.StationID).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("This Service had existed in this Station");
                }
                var serviceStation = new Station_Service
                {
                    Station_ServiceID = Guid.NewGuid(),
                    StationID = createServiceInStationModel.StationID,
                    ServiceID = createServiceInStationModel.ServiceID,
                    Price = createServiceInStationModel.Price,
                    ImageUrl = "",
                    Status = SD.ACTIVE
                };
                await _stationServiceRepository.AddAsync(serviceStation);
                await _stationServiceRepository.Commit();
                var imagePath = FirebasePathName.SERVICE_STATION + $"{serviceStation.Station_ServiceID}";
                var imageUploadResult = await _firebaseService.UploadFileToFirebase(createServiceInStationModel.ImageUrl, imagePath);
                if (imageUploadResult.IsSuccess)
                {
                    serviceStation.ImageUrl = (string)imageUploadResult.Result;
                }

                _stationServiceRepository.Update(serviceStation);
                var rs = await _stationServiceRepository.Commit();
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
        public async Task<bool> UpdateServiceStation(UpdateServiceInStationModel updateServiceInStationModel)
        {
            try
            {
                var serviceStation = await _stationServiceRepository.GetByIdAsync(updateServiceInStationModel.Station_ServiceID);
                var checkExisted = await _stationServiceRepository.FindByCondition(_ =>
                                         _.ServiceID == updateServiceInStationModel.ServiceID &&
                                         _.StationID == updateServiceInStationModel.StationID)
                                           .FirstOrDefaultAsync();              

                if (checkExisted.Station_ServiceID != updateServiceInStationModel.Station_ServiceID)
                {
                    var checkDuplicate = await _stationServiceRepository.FindByCondition(_ =>
                        _.ServiceID == updateServiceInStationModel.ServiceID &&
                        _.StationID == updateServiceInStationModel.StationID &&
                        _.Station_ServiceID != updateServiceInStationModel.Station_ServiceID)
                        .AnyAsync();

                    if (checkDuplicate)
                    {
                        throw new BadRequestException("This Service already exists in this Station");
                    }
                }

                serviceStation.StationID = updateServiceInStationModel.StationID;
                serviceStation.ServiceID = updateServiceInStationModel.ServiceID;
                serviceStation.Price = updateServiceInStationModel.Price;
                _stationServiceRepository.Update(serviceStation);
                await _stationServiceRepository.Commit();
                if (updateServiceInStationModel.ImageUrl != null && updateServiceInStationModel.ImageUrl.Length > 0)
                {
                    if (!string.IsNullOrEmpty(serviceStation.ImageUrl))
                    {
                        string url = $"{FirebasePathName.SERVICE_STATION}{serviceStation.Station_ServiceID}";
                        var deleteResult = await _firebaseService.DeleteFileFromFirebase(url);
                        if (!deleteResult.IsSuccess)
                        {
                            throw new InternalServerErrorException($"Failed to delete old image");
                        }
                    }
                    var imagePath = $"{FirebasePathName.SERVICE_STATION}{serviceStation.Station_ServiceID}";
                    var imageUploadResult = await _firebaseService.UploadFileToFirebase(updateServiceInStationModel.ImageUrl, imagePath);

                    if (imageUploadResult.IsSuccess)
                    {
                        serviceStation.ImageUrl = (string)imageUploadResult.Result;
                    }
                    else
                    {
                        throw new InternalServerErrorException($"Failed to upload new image:");
                    }

                    _stationServiceRepository.Update(serviceStation);
                }
                var rs = await _stationServiceRepository.Commit();
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
        public async Task<bool> ChangeStatusServiceInStation(Guid Station_ServiceID)
        {
            try
            {
                var serviceStation = await _stationServiceRepository.GetByIdAsync(Station_ServiceID);
                if (serviceStation == null)
                {
                    throw new NotFoundException("Service not found.");
                }
                serviceStation.Status = SD.INACTIVE;
                _stationServiceRepository.Update(serviceStation);
                var rs = await _stationServiceRepository.Commit();
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
