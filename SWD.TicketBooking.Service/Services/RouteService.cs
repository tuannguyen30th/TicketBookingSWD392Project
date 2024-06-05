using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new Exception();
            }
        }

    }
}
