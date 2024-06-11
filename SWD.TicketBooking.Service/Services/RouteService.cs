using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Route = SWD.TicketBooking.Repo.Entities.Route;

namespace SWD.TicketBooking.Service.Services
{
    public class RouteService
    {
        private readonly IRepository<Route, Guid> _routeRepo;
        private readonly IRepository<Company, Guid> _companyRepo;
        private readonly IRepository<Route_Company, Guid> _routeCompanyRepo;
        private readonly IMapper _mapper;

        public RouteService(IRepository<Route, Guid> routeRepo, IRepository<Company, Guid> companyRepo, IRepository<Route_Company, Guid> routeCompanyRepo, IMapper mapper)
        {
            _routeRepo = routeRepo;
            _companyRepo = companyRepo;
            _routeCompanyRepo = routeCompanyRepo;
            _mapper = mapper;
        }

        public async Task<List<RouteModel>> GetAllRoutes()
        {
            try
            {
                var routes = await _routeRepo.FindByCondition(_ => _.Status.Trim().Equals(SD.ACTIVE)).ToListAsync();
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
                var checkCompanyExisted = await _companyRepo.GetAll().Where(_ => _.CompanyID == model.CompanyID && _.Status.ToLower().Trim().Equals("active")).FirstOrDefaultAsync();
                if (checkCompanyExisted == null)
                {
                    throw new BadRequestException("Company does not exist!");
                }

                var checkRouteExisted = await _routeRepo.GetAll().Where(_ => _.FromCityID == model.FromCityID 
                                                                && _.ToCityID == model.ToCityID
                                                                && _.StartLocation == model.StartLocation && _.EndLocation == model.EndLocation).FirstOrDefaultAsync();
                if (checkRouteExisted == null)
                {
                    var route = await _routeRepo.AddAsync(new Route
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
                    await _routeRepo.Commit();
                }
                else if (!checkRouteExisted.Status.Trim().Equals(SD.ACTIVE))
                {
                    throw new BadRequestException("Route is not available");
                }

                var getRoute = await _routeRepo.GetAll().Where(_ => _.FromCityID == model.FromCityID
                                                && _.ToCityID == model.ToCityID
                                                && _.StartLocation == model.StartLocation && _.EndLocation == model.EndLocation).FirstOrDefaultAsync();

                if (getRoute == null)
                {
                    throw new InternalServerErrorException("Some errors occured");
                }

                var checkRouteCompanyExisted = await _routeCompanyRepo.GetAll().Where(_ => _.RouteID == getRoute.RouteID && _.CompanyID == model.CompanyID).FirstOrDefaultAsync();

                if (checkRouteCompanyExisted == null && getRoute != null)
                {
                    var routeCompany = await _routeCompanyRepo.AddAsync(new Route_Company
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

                    var rs = await _routeCompanyRepo.Commit();

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
                var checkExisted = await _routeRepo.GetAll().Where(_ => _.FromCityID == model.FromCityID
                                                                && _.ToCityID == model.ToCityID
                                                                && _.StartLocation == model.StartLocation && _.EndLocation == model.EndLocation).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Route already existed");
                }

                var entity = await _routeRepo.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE) && _.RouteID == routeId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find route");
                }

                entity.FromCityID = model.FromCityID;
                entity.ToCityID = model.ToCityID;
                entity.StartLocation = model.StartLocation;
                entity.EndLocation = model.EndLocation;

                var companyUpdate = _routeRepo.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                var rs = await _routeRepo.Commit();
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
                var entity = await _routeRepo.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE) && _.RouteID == routeId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find route");
                }

                entity.Status = status;

                var companyUpdate = _routeRepo.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                var rs = await _routeRepo.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
