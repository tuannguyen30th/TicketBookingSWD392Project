using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class RouteService
    {
        private readonly IRepository<Route, int> _routeRepo;
        private readonly IMapper _mapper;

        public RouteService(IRepository<Route, int> routeRepo, IMapper mapper)
        {
            _routeRepo = routeRepo;
            _mapper = mapper;
        }

        public async Task<List<RouteModel>> GetAllRoutes()
        {
            try
            {
                var routes = await _routeRepo.FindByCondition(_ => _.Status.ToLower().Trim() == "active").ToListAsync();
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
                var checkExisted = await _routeRepo.GetAll().Where(_ => _.FromCityID == model.FromCityID 
                                                                && _.ToCityID == model.ToCityID
                                                                && _.StartLocation == model.StartLocation && _.EndLocation == model.EndLocation).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Route existed");
                }
                var company = await _routeRepo.AddAsync(new Route
                {
                    FromCityID = model.FromCityID,
                    ToCityID = model.ToCityID,
                    StartLocation = model.StartLocation,
                    EndLocation = model.EndLocation,
                    Status = "Active"
                });
                if (company == null)
                {
                    throw new InternalServerErrorException("Cannot create");
                }
                var rs = await _routeRepo.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateRoute(int routeId, CreateRouteModel model)
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

                var entity = await _routeRepo.GetAll().Where(_ => _.Status.ToLower().Trim() == "active" && _.RouteID == routeId).FirstOrDefaultAsync();

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

        public async Task<int> ChangeStatus(int routeId, string status)
        {
            try
            {
                var entity = await _routeRepo.GetAll().Where(_ => _.Status.ToLower().Trim() == "active" && _.RouteID == routeId).FirstOrDefaultAsync();

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
