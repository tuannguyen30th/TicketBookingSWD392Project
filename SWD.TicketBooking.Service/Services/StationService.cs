using AutoMapper;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class StationService : IStationService
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StationService(IUnitOfWork unitOfWork,  IMapper mapper, IFirebaseService firebaseService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseService = firebaseService;
        }

        public async Task<List<GetStationByCompanyModel>> GetAllStationsByCompanyID(Guid companyID)
        {
            try
            {
                var stations = await _unitOfWork.StationRepository
                                                .FindByCondition(_ => _.CompanyID.Equals(companyID) && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                .ToListAsync();

                var result = new List<GetStationByCompanyModel>();
                foreach (var station in stations)
                {
                    var listServiceInStation = await _unitOfWork.Station_ServiceRepository
                                                    .GetAll()
                                                    .Where(s => s.StationID.Equals(station.StationID))
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

                    var stationResult = new GetStationByCompanyModel
                    {
                        StationID = station.StationID,
                        CityID = (Guid)station.CityID,
                        StationName = station.Name,
                        ServiceTypeInStation = serviceTypeList
                    };

                    result.Add(stationResult);
                }
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<StationFromRouteModel>> GetStationsFromRoute(Guid routeID)
        {
            try
            {
                var stations = await _unitOfWork.Station_RouteRepository
                                                .FindByCondition(_ => _.RouteID == routeID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                .Include(_ => _.Station)
                                                .Select(_ => new StationFromRouteModel
                                                {
                                                    StationID = (Guid)_.StationID,
                                                    Name = _.Station.Name
                                                })
                                                .ToListAsync();

                return stations;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<GetStationModel>> GetAllStationActive()
        {
            try
            {
                var station = await _unitOfWork.StationRepository.GetAll().ToListAsync();
                var rs = _mapper.Map<List<GetStationModel>>(station);
                return rs;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public async Task<GetStationModel> GetStationById(Guid id)
        {
            try
            {
                var station = await _unitOfWork.StationRepository.GetByIdAsync(id);
                var rs = _mapper.Map<GetStationModel>(station);
                return rs;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public async Task<string> CreateStation(CreateStationModel stationModel)
        {
            try
            {
                var check = await _unitOfWork.StationRepository
                                             .GetAll().Where(s =>s.Name.Equals(stationModel.StationName))
                                             .FirstOrDefaultAsync();
                if (check == null)
                {
                    var company = await _unitOfWork.CompanyRepository.GetByIdAsync(stationModel.CompanyId);
                    var city = await _unitOfWork.CityRepository.GetByIdAsync(stationModel.CityId);
                    var station = await _unitOfWork.StationRepository.AddAsync(new Station 
                            {   
                                CityID = city.CityID,
                                CompanyID = company.CompanyID,
                                Name = stationModel.StationName, 
                                Status = SD.GeneralStatus.ACTIVE,
                            });
                    if (station == null)
                    {
                        throw new InternalServerErrorException(SD.Notification.Internal("TRẠM", "KHI KHÔNG THỂ TẠO MỚI TRẠM NÀY"));
                    }

                    var checkout_Route = await _unitOfWork.RouteRepository
                                                          .GetAll()
                                                          .Where(r => r.RouteID.Equals(stationModel.RouteId) && r.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                          .FirstOrDefaultAsync();
                    var station_route = new Station_Route
                    {
                        Station_RouteID = new Guid(),
                        RouteID = checkout_Route.RouteID,
                        StationID = station.StationID,
                        Status = SD.GeneralStatus.ACTIVE,
                        OrderInRoute = stationModel.OrderInRoute
                    };
                    
                    _unitOfWork.Complete();
                    return "OK";
                }
                else throw new BadRequestException("TRẠM NÀY ĐÃ TỒN TẠI!");

            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public async Task<bool> CreateStationWithService(CreateStationWithServiceModel reqModel)
        {
            try
            {
                var check = await _unitOfWork.StationRepository
                                             .GetAll()
                                             .Where(s => s.Name.Equals(reqModel.StationName))
                                             .FirstOrDefaultAsync();
                if (check == null)
                {
                    var company = await _unitOfWork.CompanyRepository.GetByIdAsync(reqModel.CompanyID);
                    var city = await _unitOfWork.CityRepository.GetByIdAsync(reqModel.CityID);
                    var station = await _unitOfWork.StationRepository.AddAsync(new Station
                    {
                        StationID = Guid.NewGuid(),
                        CityID = city.CityID,
                        CompanyID = company.CompanyID,
                        Name = reqModel.StationName,
                        Status = SD.GeneralStatus.ACTIVE,
                    });
                    if (station == null)
                    {
                        throw new InternalServerErrorException(SD.Notification.Internal("TRẠM", "KHI KHÔNG THỂ TẠO MỚI TRẠM NÀY"));
                    }

                    foreach(var service in reqModel.ServiceToCreateModels)
                    { 
                        var createStationService = new Station_Service
                        {
                            Station_ServiceID = Guid.NewGuid(),
                            ServiceID = service.ServiceID,
                            StationID = station.StationID,
                            Price = service.Price,
                            Status = SD.GeneralStatus.ACTIVE,
                        };

                        var imagePath = FirebasePathName.SERVICE_STATION + $"{createStationService.Station_ServiceID}";
                        var imageUploadResult = await _firebaseService.UploadFileToFirebase(service.Image, imagePath);
                        if (!imageUploadResult.IsSuccess)
                        {
                            throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHI TẢI LÊN"));
                        }

                        createStationService.ImageUrl = (string)imageUploadResult.Result;
                        await _unitOfWork.Station_ServiceRepository.AddAsync(createStationService);
                    }

                    var rs = _unitOfWork.Complete();
                    return rs < 1 ? false : true;
                }
                else throw new BadRequestException("TRẠM NÀY ĐÃ TỒN TẠI!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<string> UpdateStation(Guid stationId, UpdateStationModel stationModel)
        {
            try
            {
                var check  = await _unitOfWork.StationRepository
                                              .GetAll()
                                              .Where(s=> s.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && s.StationID == stationId)
                                              .FirstOrDefaultAsync();
                if (check == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("TRẠM"));
                }
                else
                {
                    var checkName = await _unitOfWork.StationRepository
                                                     .GetAll()
                                                     .Where(s => s.Name.ToLower().Equals(stationModel.StationName))
                                                     .FirstOrDefaultAsync();
                    if (checkName == null)
                    {
                        check.Name = stationModel.StationName;
                        _unitOfWork.StationRepository.Update(check);
                        _unitOfWork.Complete();
                    }
                    return "OK";
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<List<StationFromRouteModel>> GetAllStationInRoute(Guid id)
        {
            try
            {
                var route = await _unitOfWork.TripRepository
                                             .FindByCondition(s => s.TripID == id && s.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)
                                                               && s.Route_Company.Route.Status.Equals(SD.GeneralStatus.ACTIVE))
                                             .Include(_ => _.Route_Company)
                                             .Select(s => s.Route_Company.RouteID).FirstOrDefaultAsync();
                var stations = await _unitOfWork.Station_RouteRepository
                                                .FindByCondition(_ => _.RouteID == route)
                                                .Include(_ => _.Station)
                                                .OrderBy(_ => _.OrderInRoute)
                                                .Select(_ => new StationFromRouteModel
                                                {
                                                    StationID = (Guid)_.StationID,
                                                    Name = _.Station.Name
                                                })
                                                .ToListAsync();
                return stations;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
