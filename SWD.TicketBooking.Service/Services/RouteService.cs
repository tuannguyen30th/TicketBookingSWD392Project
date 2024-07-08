using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using Route = SWD.TicketBooking.Repo.Entities.Route;

namespace SWD.TicketBooking.Service.Services
{
    public class RouteService : IRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RouteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<RouteModel>> GetAllRoutes()
        {
            try
            {
                var routes = await _unitOfWork.RouteRepository
                                              .FindByCondition(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                              .ToListAsync();
                var rs = _mapper.Map<List<RouteModel>>(routes);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<GetRouteFromCompanyModel>> GetAllRouteFromCompany(Guid companyID)
        {
            try
            {
                var result = await _unitOfWork.Route_CompanyRepository
                                              .GetAll()
                                              .Where(_ => _.CompanyID == companyID)
                                              .Select(_ => new GetRouteFromCompanyModel
                                              {
                                                  Route_CompanyID = _.Route_CompanyID,
                                                  FromCity = _.Route.FromCity.Name,
                                                  ToCity = _.Route.ToCity.Name,
                                                  StartLocation = _.Route.StartLocation,
                                                  EndLocation = _.Route.EndLocation,
                                                  Status = _.Route.Status,
                                              })
                                              .ToListAsync();
                return result;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> CreateRoute(CreateRouteModel model)
        {
            try
            {
                var stations = new List<StationInRouteModel>();
                var checkCompanyExisted = await _unitOfWork.CompanyRepository
                                                           .GetAll()
                                                           .Where(_ => _.CompanyID == model.CompanyID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                           .FirstOrDefaultAsync();

                if (checkCompanyExisted == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("NHÀ XE"));
                }

                var checkRouteExisted = await _unitOfWork.RouteRepository.GetAll().Where(_ => _.FromCityID == model.FromCityID
                                                                && _.ToCityID == model.ToCityID
                                                                && _.StartLocation == model.StartLocation && _.EndLocation == model.EndLocation).FirstOrDefaultAsync();

                if (checkRouteExisted == null)
                {
                    var route = await _unitOfWork.RouteRepository.AddAsync(new Route
                    {
                        RouteID = Guid.NewGuid(),
                        FromCityID = model.FromCityID,
                        ToCityID = model.ToCityID,
                        StartLocation = model.StartLocation,
                        EndLocation = model.EndLocation,
                        Status = SD.GeneralStatus.ACTIVE
                    });
                    if (route == null)
                    {
                        throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG", "KHI TẠO MỚI TUYẾN ĐƯỜNG"));
                    }

                    checkRouteExisted = route;
                }
                else if (!checkRouteExisted.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                {
                    throw new BadRequestException("TUYẾN ĐƯỜNG KHÔNG KHẢ DỤNG!");
                }

                var checkStationCompanyExisted = new Station_Company();
                foreach (var station in model.StationInRoutes)
                {
                    checkStationCompanyExisted = await _unitOfWork.Station_CompanyRepository
                                                              .GetAll()
                                                              .Where(_ => _.CompanyID.Equals(model.CompanyID) && _.StationID.Equals(station.StationID))
                                                              .FirstOrDefaultAsync();
                    if (checkStationCompanyExisted != null)
                    {
                        throw new InternalServerErrorException(SD.Notification.Existed("NHÀ XE", "KHI TẠO MỚI TUYẾN ĐƯỜNG"));
                    }

                    if (checkStationCompanyExisted == null)
                    {
                        var stationCompany = await _unitOfWork.Station_CompanyRepository
                                                              .AddAsync(new Station_Company
                                                              {
                                                                  Station_CompanyID = Guid.NewGuid(),
                                                                  CompanyID = model.CompanyID,
                                                                  StationID = station.StationID,
                                                                  Status = SD.GeneralStatus.ACTIVE
                                                              });

                        var routeStation = await _unitOfWork.StationCompany_RouteRepository.AddAsync(new StationCompany_Route
                        {
                            Station_CompanyID = stationCompany.Station_CompanyID,
                            StationCompany_RouteID = Guid.NewGuid(),
                            OrderInRoute = station.OrderInRoute,
                            RouteID = checkRouteExisted.RouteID,
                            Status = SD.GeneralStatus.ACTIVE
                        });
                        if (routeStation == null)
                        {
                            throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG CỦA NHÀ XE", "KHI TẠO MỚI TUYẾN ĐƯỜNG CHO NHÀ XE NÀY"));
                        }
                    }
                }
                var checkRouteCompanyExisted = await _unitOfWork.Route_CompanyRepository
                                                                .GetAll()
                                                                .Where(_ => _.RouteID == checkRouteExisted.RouteID && _.CompanyID == model.CompanyID)
                                                                .FirstOrDefaultAsync();

                if (checkRouteCompanyExisted == null && checkRouteExisted != null)
                {
                    var routeCompany = await _unitOfWork.Route_CompanyRepository.AddAsync(new Route_Company
                    {
                        Route_CompanyID = Guid.NewGuid(),
                        RouteID = checkRouteExisted.RouteID,
                        CompanyID = model.CompanyID,
                        Status = SD.GeneralStatus.ACTIVE
                    });

                    if (routeCompany == null)
                    {
                        throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG CỦA NHÀ XE", "KHI TẠO MỚI TUYẾN ĐƯỜNG CHO NHÀ XE NÀY"));
                    }
                    var rs = _unitOfWork.Complete();

                    return rs;
                }
                else
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG CỦA NHÀ XE", "KHI TẠO MỚI TUYẾN ĐƯỜNG CHO NHÀ XE NÀY"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> UpdateRoute(Guid routeId, CreateRouteModel model)
        {
            try
            {
                var route = await _unitOfWork.RouteRepository
                                              .GetAll()
                                              .Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.RouteID == routeId)
                                              .FirstOrDefaultAsync();
                if (route == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG CỦA NHÀ XE", "KHI CẬP NHẬT TUYẾN ĐƯỜNG CHO NHÀ XE NÀY"));
                }
                if (route == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("TUYẾN ĐƯỜNG"));
                }
                if (model.FromCityID != Guid.Empty && model.ToCityID != Guid.Empty && !model.StartLocation.IsNullOrEmpty() && !model.EndLocation.IsNullOrEmpty())
                {
                    route.FromCityID = model.FromCityID;
                    route.ToCityID = model.ToCityID;
                    route.StartLocation = model.StartLocation;
                    route.EndLocation = model.EndLocation;

                    var companyUpdate = _unitOfWork.RouteRepository.Update(route);

                    if (companyUpdate == null)
                    {
                        throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG", "KHI CẬP NHẬT TUYẾN ĐƯỜNG NÀY"));
                    }
                }

                if (model.StationInRoutes != null)
                {

                    foreach (var station in model.StationInRoutes)
                    {
                        var checkStation = await _unitOfWork.StationRepository
                                                            .FindByCondition(s => s.StationID.Equals(station.StationID) && s.Status.Equals(SD.GeneralStatus.ACTIVE))
                                                            .FirstOrDefaultAsync();
                        if (checkStation != null)
                        { 
                           
                            var checkStationCompanyRoute = await _unitOfWork.StationCompany_RouteRepository
                                                                .FindByCondition(scr => scr.Station_CompanyID.Equals(model.StationCompany))
                                                                .FirstOrDefaultAsync();
                            checkStationCompanyRoute.OrderInRoute = station.OrderInRoute;
                            var stationRouteUpdate = _unitOfWork.StationCompany_RouteRepository.Update(checkStationCompanyRoute);
                            if ( stationRouteUpdate == null)
                            {
                                throw new InternalServerErrorException(SD.Notification.Internal("NHÀ XE", "KHI CẬP NHẬT NHÀ XE NÀY"));
                            }
                        }

                        
                    }

                }

                var rs = _unitOfWork.Complete();

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        
    }
}
