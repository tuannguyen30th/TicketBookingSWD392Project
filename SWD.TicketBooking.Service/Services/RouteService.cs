using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
                                                  EndLocation = _.Route.EndLocation
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
                    throw new BadRequestException("ĐÃ TỒN TẠI TUYẾN ĐƯỜNG CỦA NHÀ XE NÀY!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateRoute(Guid routeId, UpdateRouteModel model)
        {
            try
            {
                var checkExisted = await _unitOfWork.RouteRepository
                                                    .GetAll()
                                                    .Where(_ => _.FromCityID == model.FromCityID
                                                             && _.ToCityID == model.ToCityID
                                                             && _.StartLocation == model.StartLocation && _.EndLocation == model.EndLocation).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("ĐÃ TỒN TẠI TUYẾN ĐƯỜNG NÀY!");
                }

                var entity = await _unitOfWork.RouteRepository
                                              .GetAll()
                                              .Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.RouteID == routeId)
                                              .FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("TUYẾN ĐƯỜNG"));
                }

                entity.FromCityID = model.FromCityID;
                entity.ToCityID = model.ToCityID;
                entity.StartLocation = model.StartLocation;
                entity.EndLocation = model.EndLocation;

                var companyUpdate = _unitOfWork.RouteRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG", "KHI CẬP NHẬT TUYẾN ĐƯỜNG NÀY"));
                }
                var rs = _unitOfWork.Complete(); 

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> ChangeStatus(Guid routeId, string status)
        {
            try
            {
                var entity = await _unitOfWork.RouteRepository
                                              .GetAll()
                                              .Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.RouteID == routeId)
                                              .FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("TUYẾN ĐƯỜNG"));
                }

                entity.Status = status;

                var companyUpdate = _unitOfWork.RouteRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("TUYẾN ĐƯỜNG", "KHI CẬP NHẬT TRẠNG THÁI CHO TUYẾN ĐƯỜNG NÀY"));
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
