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
    public class StationService
    {
        private readonly IRepository<Station_Route, int> _stationRouteRepository;
        public StationService(IRepository<Station_Route, int> stationRouteRepository)
        {
            _stationRouteRepository = stationRouteRepository;
        }
        public async Task<List<StationFromRouteModel>> GetStationsFromRoute(int routeID)
        {
            try
            {
                var stations = await _stationRouteRepository
                                    .FindByCondition(_ => _.RouteID == routeID)
                                    .Include(_ => _.Station)
                                    .Select(_ => new StationFromRouteModel
                                     {
                                        StationID = _.StationID,
                                        Name = _.Station.Name
                                     })
                                     .ToListAsync();

                return stations;

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
