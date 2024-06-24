using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;

namespace SWD.TicketBooking.Service.Services
{
    public class Station_ServiceService : IStation_ServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> _unitOfWork.ServiceRepository;
        //private readonly IRepository<Station_Service, Guid> _unitOfWork.Station_ServiceRepository;
        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        public Station_ServiceService(IUnitOfWork unitOfWork, IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> serviceRepository, IRepository<Station_Service, Guid> stationServiceRepository, IFirebaseService firebaseService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_unitOfWork.ServiceRepository = serviceRepository;
            //_unitOfWork.Station_ServiceRepository = stationServiceRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }
        public async Task<bool> CreateServiceStation(CreateServiceInStationModel createServiceInStationModel)
        {
            try
            {
                var checkExisted = await _unitOfWork.Station_ServiceRepository.FindByCondition(_ => _.ServiceID == createServiceInStationModel.ServiceID && _.StationID == createServiceInStationModel.StationID).FirstOrDefaultAsync();
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
                    Status = SD.GeneralStatus.ACTIVE
                };
                await _unitOfWork.Station_ServiceRepository.AddAsync(serviceStation);
                //await _unitOfWork.Station_ServiceRepository.Commit();
                var imagePath = FirebasePathName.SERVICE_STATION + $"{serviceStation.Station_ServiceID}";
                var imageUploadResult = await _firebaseService.UploadFileToFirebase(createServiceInStationModel.ImageUrl, imagePath);
                if (imageUploadResult.IsSuccess)
                {
                    serviceStation.ImageUrl = (string)imageUploadResult.Result;
                }

                _unitOfWork.Station_ServiceRepository.Update(serviceStation);
                //var rs = await _unitOfWork.Station_ServiceRepository.Commit();
                var rs = _unitOfWork.Complete();
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
        public async Task<bool> UpdateServiceStation(UpdateServiceInStationModel updateServiceInStationModel, Guid stationServiceID)
        {
            try
            {
                var serviceStation = await _unitOfWork.Station_ServiceRepository.GetByIdAsync(stationServiceID);
                var checkExisted = await _unitOfWork.Station_ServiceRepository.FindByCondition(_ =>
                                         _.ServiceID == updateServiceInStationModel.ServiceID &&
                                         _.StationID == updateServiceInStationModel.StationID)
                                           .FirstOrDefaultAsync();              

                if (checkExisted.Station_ServiceID != updateServiceInStationModel.Station_ServiceID)
                {
                    var checkDuplicate = await _unitOfWork.Station_ServiceRepository.FindByCondition(_ =>
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
                _unitOfWork.Station_ServiceRepository.Update(serviceStation);
                //await _unitOfWork.Station_ServiceRepository.Commit();
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

                    _unitOfWork.Station_ServiceRepository.Update(serviceStation);
                }
                //var rs = await _unitOfWork.Station_ServiceRepository.Commit();
                var rs = _unitOfWork.Complete();
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
                var serviceStation = await _unitOfWork.Station_ServiceRepository.GetByIdAsync(Station_ServiceID);
                if (serviceStation == null)
                {
                    throw new NotFoundException("Service not found.");
                }
                serviceStation.Status = SD.GeneralStatus.INACTIVE;
                _unitOfWork.Station_ServiceRepository.Update(serviceStation);
                //var rs = await _unitOfWork.Station_ServiceRepository.Commit();
                var rs = _unitOfWork.Complete();
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
