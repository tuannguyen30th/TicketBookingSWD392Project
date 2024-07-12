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
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Collections.Specialized.BitVector32;

namespace SWD.TicketBooking.Service.Services
{
    public class StationService : IStationService
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StationService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseService firebaseService)
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
                                                .FindByCondition(_ => _.CompanyID.Equals(companyID))
                                                .Include(_ => _.City)
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

                    var stationResult = new GetStationByCompanyModel
                    {
                        StationID = station.StationID,
                        CityID = (Guid)station.CityID,
                        CityName = station.City.Name,
                        StationName = station.Name,
                        Status = station.Status,
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

        public async Task<List<StationFromRouteModel>> GetStationsFromTrip(Guid routeID, Guid companyID)
        {
            try
            {

                var stationsByRoute = await _unitOfWork.StationCompany_RouteRepository
                                                       .GetAll()
                                                       .Where(_ => _.RouteID == routeID && _.Station_Company.CompanyID == companyID)
                                                       .OrderBy(_ => _.OrderInRoute)
                                                       .Select(_ => _.Station_CompanyID)
                                                       .ToListAsync();
                var stationsByCompany = await _unitOfWork.Station_CompanyRepository
                                                         .FindByCondition(_ => stationsByRoute.Contains(_.Station_CompanyID)
                                                                       && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                         .Include(_ => _.Station)
                                                         .Select(_ => new StationFromRouteModel
                                                         {
                                                             Name = _.Station.Name,
                                                             StationID = (Guid)_.StationID,
                                                         })
                                                         .ToListAsync();

                return stationsByCompany;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /* public async Task<StationFromRouteModel> GetStationFromRoute(Guid routeID)
         {
             try
             {
                 var stationsByRoute = await _unitOfWork.StationCompany_RouteRepository
                 .GetAll()
                                                        .Where(_ => _.RouteID == routeID)
                                                        .OrderBy(_ => _.OrderInRoute)
                                                        .Select(_ => _.Station_CompanyID)
                                                        .ToListAsync();
             }
             catch (Exception ex)
             {
                 throw new Exception(ex.Message, ex);
             }

         }*/

        public async Task<List<GetStationModel>> GetAllStationActive()
        {
            try
            {
                var station = await _unitOfWork.StationRepository.GetAll().ToListAsync();
                var rs = _mapper.Map<List<GetStationModel>>(station);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<GetStationByCompanyModel> GetStationById(Guid stationId)
        {
            var station = await _unitOfWork.StationRepository
                                                .FindByCondition(_ => _.StationID.Equals(stationId) && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                .Include(_ => _.City)
                                                .FirstOrDefaultAsync();

            var result = new List<GetStationByCompanyModel>();

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
                CityName = station.City.Name,
                StationName = station.Name,
                Status = station.Status,
                ServiceTypeInStation = serviceTypeList
            };

            return stationResult;
        }

            //public async Task<string> CreateStation(CreateStationModel stationModel)
            //{
            //    try
            //    {
            //        var check = await _unitOfWork.StationRepository
            //                                     .GetAll().Where(s =>s.Name.Equals(stationModel.StationName))
            //                                     .FirstOrDefaultAsync();
            //        if (check == null)
            //        {
            //            var company = await _unitOfWork.CompanyRepository.GetByIdAsync(stationModel.CompanyId);
            //            var city = await _unitOfWork.CityRepository.GetByIdAsync(stationModel.CityId);
            //            var station = await _unitOfWork.StationRepository.AddAsync(new Station 
            //                    {   
            //                        CityID = city.CityID,
            //                        CompanyID = company.CompanyID,
            //                        Name = stationModel.StationName, 
            //                        Status = SD.GeneralStatus.ACTIVE,
            //                    });
            //            if (station == null)
            //            {
            //                throw new InternalServerErrorException(SD.Notification.Internal("TRẠM", "KHI KHÔNG THỂ TẠO MỚI TRẠM NÀY"));
            //            }

            //            var checkout_Route = await _unitOfWork.RouteRepository
            //                                                  .GetAll()
            //                                                  .Where(r => r.RouteID.Equals(stationModel.RouteId) && r.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
            //                                                  .FirstOrDefaultAsync();
            //            var station_route = new StationCompany_Route
            //            {
            //                Station_RouteID = new Guid(),
            //                RouteID = checkout_Route.RouteID,
            //                StationID = station.StationID,
            //                Status = SD.GeneralStatus.ACTIVE,
            //                OrderInRoute = stationModel.OrderInRoute
            //            };

            //            _unitOfWork.Complete();
            //            return "OK";
            //        }
            //        else throw new BadRequestException("TRẠM NÀY ĐÃ TỒN TẠI!");

            //    } catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message,ex);
            //    }
            //}

            public async Task<bool> CreateStation(CreateStationModel stationModel)
            {
                try
                {
                    var check = await _unitOfWork.StationRepository
                                                 .GetAll()
                                                 .Where(s => s.Name.Equals(stationModel.StationName)
                                                                && s.CityID.Equals(stationModel.CityId)
                                                                && s.CompanyID.Equals(stationModel.CompanyId))
                                                 .FirstOrDefaultAsync();
                    if (check == null)
                    {
                        var company = await _unitOfWork.CompanyRepository.GetByIdAsync(stationModel.CompanyId);
                        if (company == null)
                        {
                            throw new BadRequestException(SD.Notification.NotFoundByField("NHÀ XE", "ID"));
                        }
                        var city = await _unitOfWork.CityRepository.GetByIdAsync(stationModel.CityId);
                        if (city == null)
                        {
                            throw new BadRequestException(SD.Notification.NotFoundByField("THÀNH PHỐ", "ID"));
                        }
                        var station = await _unitOfWork.StationRepository.AddAsync(new Station
                        {
                            StationID = new Guid(),
                            CityID = city.CityID,
                            CompanyID = company.CompanyID,
                            Name = stationModel.StationName,
                            Status = SD.GeneralStatus.ACTIVE,
                        });
                        if (station == null)
                        {
                            throw new InternalServerErrorException(SD.Notification.Internal("TRẠM", "KHI KHÔNG THỂ TẠO MỚI TRẠM NÀY"));
                        }

                        var rs = _unitOfWork.Complete();
                        return rs > 0 ? true : false;
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
                    var check = await _unitOfWork.StationRepository
                                                  .GetAll()
                                                  .Where(s => s.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && s.StationID == stationId)
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
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            public async Task<bool> CompanyRegisStation(Guid companyID, List<Guid> stationID)
            {
                try
                {
                    var check = await _unitOfWork.CompanyRepository.GetByIdAsync(companyID);
                    if (check != null)
                    {
                        var uniqueIDs = new HashSet<Guid>();
                        foreach (var id in stationID)
                        {
                            if (!uniqueIDs.Add(id))
                            {
                                throw new BadRequestException($"Duplicate stationID found: {id}");
                            }

                            var checkStation = await _unitOfWork.StationRepository.GetByIdAsync(id);
                            if (checkStation == null)
                            {
                                throw new BadRequestException(SD.Notification.NotFoundByField("TRẠM", id.ToString().ToUpper()));
                            }

                            var checkExisted = await _unitOfWork.Station_CompanyRepository
                                                                .GetAll()
                                                                .Where(_ => _.StationID.Equals(id) && _.CompanyID.Equals(companyID))
                                                                .FirstOrDefaultAsync();

                            if (checkExisted != null)
                            {
                                if (checkExisted.Status.Equals(SD.GeneralStatus.ACTIVE))
                                {
                                    continue;
                                }
                                else
                                {
                                    checkExisted.Status = SD.GeneralStatus.ACTIVE;
                                    _unitOfWork.Station_CompanyRepository.Update(checkExisted);
                                    continue;
                                }
                            }

                            var station_company = await _unitOfWork.Station_CompanyRepository.AddAsync(new Station_Company
                            {
                                Station_CompanyID = new Guid(),
                                StationID = id,
                                CompanyID = companyID,
                                Status = SD.GeneralStatus.ACTIVE,
                            });

                            if (station_company == null)
                            {
                                throw new InternalServerErrorException(SD.Notification.Internal("TRẠM", "KHI KHÔNG THỂ TẠO MỚI TRẠM NÀY"));
                            }
                        }

                        _unitOfWork.Complete();
                        return true;
                    }
                    else throw new BadRequestException(SD.Notification.NotFoundByField("NHÀ XE", "ID"));

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            public async Task<List<GetStationModel>> GetStationsByCityId(Guid cityID)
            {
                try
                {
                    var stations = await _unitOfWork.StationRepository
                                                   .GetAll()
                                                   .Where(_ => _.CityID.Equals(cityID) && _.Status.Equals(SD.GeneralStatus.ACTIVE))
                                                   .ToListAsync();

                    var rs = _mapper.Map<List<GetStationModel>>(stations);
                    return rs;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }

            /*   public async Task<List<StationFromRouteModel>> GetAllStationInRoute(Guid id)
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
               }*/

        }
    }
