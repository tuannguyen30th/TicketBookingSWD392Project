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

        public async Task<List<PopularRouteModel>> GetPopularRoutes()
        {
            try
            {
                var test = await _unitOfWork.BookingRepository
                    .GetAll()
                    .GroupBy(b => b.TripID)
                    .Select(g => new
                    {
                        TripID = g.Key,
                        TotalBooking = g.Count()
                    })
                    .ToListAsync();

                var routeIDs = await _unitOfWork.TripRepository
                    .GetAll()
                    .Where(t => test.Select(tt => tt.TripID).Contains(t.TripID))
                    .Include(t => t.Route_Company.Route)
                    .Select(t => t.Route_Company.RouteID)
                    .Distinct()
                    .ToListAsync();

                var allTrips = _unitOfWork.TripRepository
                    .GetAll()
                    .Include(tr => tr.Route_Company.Route)
                    .ToList();

                var topRoutes = allTrips
                    .GroupBy(tr => tr.Route_Company.RouteID)
                    .Select(g => new
                    {
                        RouteID = g.Key,
                        TotalBookings = g.Sum(tr => test.Where(t => t.TripID == tr.TripID).Sum(t => t.TotalBooking))
                    })
                    .OrderByDescending(r => r.TotalBookings)
                    .Take(5)
                    .ToList();


                var rs = new List<PopularRouteModel>();

                foreach(var route in topRoutes)
                {
                    var getRoute = await _unitOfWork.RouteRepository
                                                    .GetAll()
                                                    .Include(_ => _.FromCity)
                                                    .Include(_ => _.ToCity)
                                                    .Where(_ => _.RouteID.Equals((Guid)route.RouteID))
                                                    .FirstOrDefaultAsync();

                    var routeRs = new PopularRouteModel
                    {
                        RouteID = getRoute.RouteID,
                        FromCityID = getRoute.FromCity.CityID,
                        FromCity = getRoute.FromCity.Name,
                        ToCityID = getRoute.ToCity.CityID,
                        ToCity = getRoute.ToCity.Name,
                        StartLocation = getRoute.StartLocation,
                        EndLocation = getRoute.EndLocation,
                        TotalBooking = route.TotalBookings
                    };

                    rs.Add(routeRs);
                }

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
                                              .Include(_ => _.Route)
                                              .Where(_ => _.CompanyID == companyID)
                                              .Select(_ => new GetRouteFromCompanyModel
                                              {
                                                  Route_CompanyID = _.Route_CompanyID,
                                                  RouteID = _.Route.RouteID,
                                                  FromCity = _.Route.FromCity.Name,
                                                  ToCity = _.Route.ToCity.Name,
                                                  StartLocation = _.Route.StartLocation,
                                                  EndLocation = _.Route.EndLocation,
                                                  Status = _.Status,
                                              })
                                              .ToListAsync();
                return result;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<CreateRouteResponse> CreateRoute(CreateRouteModel model)
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
                var routeID = Guid.NewGuid();
                if (checkRouteExisted == null)
                {
                    var route = await _unitOfWork.RouteRepository.AddAsync(new Route
                    {
                        RouteID = routeID,
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
                    _unitOfWork.Complete();

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
                        checkStationCompanyExisted = stationCompany;
                    }

                    var routeStation = await _unitOfWork.StationCompany_RouteRepository.AddAsync(new StationCompany_Route
                    {
                        Station_CompanyID = checkStationCompanyExisted.Station_CompanyID,
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
                    _unitOfWork.Complete();
                    var fromCity = await _unitOfWork.CityRepository
                                                  .FindByCondition(c => c.CityID.Equals(checkRouteExisted.FromCityID))
                                                  .FirstOrDefaultAsync();
                    var toCity = await _unitOfWork.CityRepository
                                                  .FindByCondition(c => c.CityID.Equals(checkRouteExisted.ToCityID))
                                                  .FirstOrDefaultAsync();
                    var rs = new CreateRouteResponse
                    {
                        RouteID = checkRouteExisted.RouteID,
                        FromCity = fromCity.Name,
                        ToCity = toCity.Name,
                        EndLocation = checkRouteExisted.EndLocation,
                        StartLocation = checkRouteExisted.StartLocation,
                        Status = checkRouteExisted.Status,
                        Route_CompanyID = routeCompany.Route_CompanyID
                    };
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
                    var checkStationCompanyExisted = new Station_Company();

                    foreach (var station in model.StationInRoutes)
                    {
                        checkStationCompanyExisted = await _unitOfWork.Station_CompanyRepository
                                                                  .GetAll()
                                                                  .Where(_ => _.CompanyID.Equals(model.CompanyID) && _.StationID.Equals(station.StationID))
                                                                  .FirstOrDefaultAsync();

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
                            checkStationCompanyExisted = stationCompany;
                            var routeStation = await _unitOfWork.StationCompany_RouteRepository.AddAsync(new StationCompany_Route
                            {
                                Station_CompanyID = checkStationCompanyExisted.Station_CompanyID,
                                StationCompany_RouteID = Guid.NewGuid(),
                                OrderInRoute = station.OrderInRoute,
                                RouteID = route.RouteID,
                                Status = SD.GeneralStatus.ACTIVE
                            });
                            if (routeStation == null)
                            {
                                throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG CỦA NHÀ XE", "KHI TẠO MỚI TUYẾN ĐƯỜNG CHO NHÀ XE NÀY"));
                            }
                        }
                        else
                        {
                            var checkStationCompanyRoute = await _unitOfWork.StationCompany_RouteRepository
                                                                        .FindByCondition(scr => scr.Station_CompanyID.Equals(checkStationCompanyExisted.Station_CompanyID) && scr.RouteID.Equals(route.RouteID))
                                                                        .FirstOrDefaultAsync();
                            if (checkStationCompanyRoute != null)
                            {
                                checkStationCompanyRoute.OrderInRoute = station.OrderInRoute;
                                _unitOfWork.StationCompany_RouteRepository.Update(checkStationCompanyRoute);
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
