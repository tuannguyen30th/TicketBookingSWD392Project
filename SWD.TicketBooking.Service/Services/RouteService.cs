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
        //private readonly IRepository<Route, Guid> _unitOfWork.RouteRepository;
        //private readonly IRepository<Company, Guid> _unitOfWork.CompanyRepository;
        //private readonly IRepository<Route_Company, Guid> _unitOfWork.Route_CompanyRepository;
        private readonly IMapper _mapper;

        public RouteService(IUnitOfWork unitOfWork, IRepository<Route, Guid> routeRepo, IRepository<Company, Guid> companyRepo, IRepository<Route_Company, Guid> routeCompanyRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_unitOfWork.RouteRepository = routeRepo;
            //_unitOfWork.CompanyRepository = companyRepo;
            //_unitOfWork.Route_CompanyRepository = routeCompanyRepo;
            _mapper = mapper;
        }

        public async Task<List<RouteModel>> GetAllRoutes()
        {
            try
            {
                var routes = await _unitOfWork.RouteRepository.FindByCondition(_ => _.Status.Trim().Equals(SD.ACTIVE)).ToListAsync();
                var rs = _mapper.Map<List<RouteModel>>(routes);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> CreateRoute(CreateRouteModel model)
        {
            try
            {
                var checkCompanyExisted = await _unitOfWork.CompanyRepository.GetAll().Where(_ => _.CompanyID == model.CompanyID && _.Status.ToLower().Trim().Equals("active")).FirstOrDefaultAsync();
                if (checkCompanyExisted == null)
                {
                    throw new BadRequestException("Company does not exist!");
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
                        Status = SD.ACTIVE
                    });
                    if (route == null)
                    {
                        throw new InternalServerErrorException("Cannot create!");
                    }
                    //await _unitOfWork.RouteRepository.Commit();
                }
                else if (!checkRouteExisted.Status.Trim().Equals(SD.ACTIVE))
                {
                    throw new BadRequestException("Route is not available");
                }

                var getRoute = await _unitOfWork.RouteRepository.GetAll().Where(_ => _.FromCityID == model.FromCityID
                                                && _.ToCityID == model.ToCityID
                                                && _.StartLocation == model.StartLocation && _.EndLocation == model.EndLocation).FirstOrDefaultAsync();

                if (getRoute == null)
                {
                    throw new InternalServerErrorException("Some errors occured");
                }

                var checkRouteCompanyExisted = await _unitOfWork.Route_CompanyRepository.GetAll().Where(_ => _.RouteID == getRoute.RouteID && _.CompanyID == model.CompanyID).FirstOrDefaultAsync();

                if (checkRouteCompanyExisted == null && getRoute != null)
                {
                    var routeCompany = await _unitOfWork.Route_CompanyRepository.AddAsync(new Route_Company
                    {
                        Route_CompanyID = Guid.NewGuid(),
                        RouteID = getRoute.RouteID,
                        CompanyID = model.CompanyID,
                        Status = SD.ACTIVE
                    });

                    if (routeCompany == null)
                    {
                        throw new InternalServerErrorException("Cannot create");
                    }

                    //var rs = await _unitOfWork.Route_CompanyRepository.Commit();
                    var rs = _unitOfWork.Complete();

                    return rs;
                }
                else 
                {
                    throw new BadRequestException("Route already existed");
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
                var checkExisted = await _unitOfWork.RouteRepository.GetAll().Where(_ => _.FromCityID == model.FromCityID
                                                                && _.ToCityID == model.ToCityID
                                                                && _.StartLocation == model.StartLocation && _.EndLocation == model.EndLocation).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Route already existed");
                }

                var entity = await _unitOfWork.RouteRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE) && _.RouteID == routeId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find route");
                }

                entity.FromCityID = model.FromCityID;
                entity.ToCityID = model.ToCityID;
                entity.StartLocation = model.StartLocation;
                entity.EndLocation = model.EndLocation;

                var companyUpdate = _unitOfWork.RouteRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                //var rs = await _unitOfWork.RouteRepository.Commit();
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
                var entity = await _unitOfWork.RouteRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE) && _.RouteID == routeId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find route");
                }

                entity.Status = status;

                var companyUpdate = _unitOfWork.RouteRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                //var rs = await _unitOfWork.RouteRepository.Commit();
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
