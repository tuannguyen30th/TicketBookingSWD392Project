using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        public Station_ServiceService(IUnitOfWork unitOfWork, IFirebaseService firebaseService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }
        public async Task<bool> CreateServiceStation(CreateServiceInStationModel createServiceInStationModel)
        {
            try
            {
                var checkExisted = await _unitOfWork.Station_ServiceRepository
                                                    .FindByCondition(_ => _.ServiceID == createServiceInStationModel.ServiceID && _.StationID == createServiceInStationModel.StationID)
                                                    .FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("DỊCH VỤ ĐÃ TỒN TẠI Ở TRẠM NÀY!");
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
                var imagePath = FirebasePathName.SERVICE_STATION + $"{serviceStation.Station_ServiceID}";
                var imageUploadResult = await _firebaseService.UploadFileToFirebase(createServiceInStationModel.ImageUrl, imagePath);
                if (imageUploadResult.IsSuccess)
                {
                    serviceStation.ImageUrl = (string)imageUploadResult.Result;
                }
                await _unitOfWork.Station_ServiceRepository.AddAsync(serviceStation);
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

        public async Task<List<ServiceTypeInStationModel>> AddServiceIntoStation(AddServiceToStationModel reqModel)
        {
            try
            {
                var check = await _unitOfWork.StationRepository
                                             .GetAll()
                                             .Where(s => s.StationID.Equals(reqModel.StationID))
                                             .FirstOrDefaultAsync();
                if (check != null)
                {
                    foreach (var service in reqModel.ServiceToCreateModels)
                    {
                        if(service.ServiceTypeID != null && !service.Name.IsNullOrEmpty() && service.ServiceID == null)
                        {
                            var createService = new SWD.TicketBooking.Repo.Entities.Service
                            {
                                ServiceID = Guid.NewGuid(),
                                Name = service.Name,
                                ServiceTypeID = service.ServiceTypeID,
                                Status = SD.GeneralStatus.ACTIVE
                            };

                            var serviceRs = await _unitOfWork.ServiceRepository.AddAsync(createService);

                            service.ServiceID = serviceRs.ServiceID;
                        }

                        var createStationService = new Station_Service
                        {
                            Station_ServiceID = Guid.NewGuid(),
                            ServiceID = service.ServiceID,
                            StationID = reqModel.StationID,
                            Price = service.Price,
                            Status = SD.GeneralStatus.ACTIVE,
                        };

                        if (service.Image != null)
                        {
                            var imagePath = FirebasePathName.SERVICE_STATION + $"{createStationService.Station_ServiceID}";
                            var imageUploadResult = await _firebaseService.UploadFileToFirebase(service.Image, imagePath);
                            if (!imageUploadResult.IsSuccess)
                            {
                                throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHI TẢI LÊN"));
                            }

                            createStationService.ImageUrl = (string)imageUploadResult.Result;
                            await _unitOfWork.Station_ServiceRepository.AddAsync(createStationService);
                        }
                        else
                        {
                            throw new BadRequestException($"DỊCH VỤ {service.Name.ToUpper()} CHƯA CÓ HÌNH ẢNH!");
                        }
                    }

                    var rs = _unitOfWork.Complete();

                    var listServiceInStation = await _unitOfWork.Station_ServiceRepository
                                                    .GetAll()
                                                    .Where(s => s.StationID.Equals(reqModel.StationID))
                                                    .ToListAsync();

                    var listServices = await _unitOfWork.ServiceRepository
                                                        .GetAll()
                                                        .Where(s => listServiceInStation.Select(s => s.ServiceID).Contains(s.ServiceID))
                                                        .ToListAsync();

                    var serviceType = await _unitOfWork.ServiceTypeRepository
                                                     .GetAll()
                                                     .Where(s => listServices.Select(s => s.ServiceTypeID).Contains(s.ServiceTypeID))
                                                     .ToListAsync();

                    var serviceTypeList = new List<ServiceTypeInStationModel>();
                    Parallel.ForEach(serviceType, async (item) =>
                    {
                        var listServiceInServiceType = listServices.Where(s => s.ServiceTypeID.Equals(item.ServiceTypeID)).ToList();

                        var listServiceInStationModelTask = listServiceInServiceType.Select(async s => new ServiceInStationModel
                        {
                            Service_StationID = listServiceInStation.Where(p => p.ServiceID.Equals(s.ServiceID)).FirstOrDefault().Station_ServiceID,
                            ServiceID = s.ServiceID,
                            Name = s.Name,
                            Price = (double)listServiceInStation.Where(p => p.ServiceID.Equals(s.ServiceID)).FirstOrDefault().Price,
                            ImageUrl = listServiceInStation.Where(p => p.ServiceID.Equals(s.ServiceID)).FirstOrDefault().ImageUrl
                        }).ToList();

                        var listServiceInStationModel = await Task.WhenAll(listServiceInStationModelTask);

                        var serviceResponse = new ServiceTypeInStationModel
                        {
                            ServiceTypeID = item.ServiceTypeID,
                            ServiceTypeName = item.Name,
                            ServiceInStation = listServiceInStationModel.ToList(),
                        };

                        serviceTypeList.Add(serviceResponse);
                    });

                    return serviceTypeList;
                }
                else throw new BadRequestException(SD.Notification.NotFoundByField("TRẠM", "ID"));
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

                if (serviceStation == null)
                {
                    throw new BadRequestException(SD.Notification.Existed("TRẠM", "DỊCH VỤ"));
                }

                serviceStation.Price = updateServiceInStationModel.Price;
                if (updateServiceInStationModel.ImageUrl != null && updateServiceInStationModel.ImageUrl.Length > 0)
                {
                    if (!string.IsNullOrEmpty(serviceStation.ImageUrl))
                    {
                        string url = $"{FirebasePathName.SERVICE_STATION}{serviceStation.Station_ServiceID}";
                        var deleteResult = await _firebaseService.DeleteFileFromFirebase(url);
                        if (!deleteResult.IsSuccess)
                        {
                            throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHI XÓA"));
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
                        throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHI TẢI LÊN"));
                    }

                    _unitOfWork.Station_ServiceRepository.Update(serviceStation);
                }
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
                    throw new NotFoundException(SD.Notification.NotFound("DỊCH VỤ"));
                }
                serviceStation.Status = SD.GeneralStatus.INACTIVE;
                _unitOfWork.Station_ServiceRepository.Update(serviceStation);
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
