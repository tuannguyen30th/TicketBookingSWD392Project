using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IStationService
    {
        Task<List<StationFromRouteModel>> GetStationsFromRoute(Guid routeID);
        Task<List<GetStationModel>> GetAllStationActive();
        Task<GetStationModel> GetStationById(Guid id);
        Task<string> CreateStation(CreateStationModel stationModel);
        Task<string> UpdateStation(Guid stationId, CreateStationModel stationModel);
        Task<List<StationFromRouteModel>> GetAllStationInRoute(Guid id);
        Task<bool> CreateStationWithService(CreateStationWithServiceModel stationModel);
    }
}
