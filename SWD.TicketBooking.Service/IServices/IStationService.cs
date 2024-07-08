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
        Task<List<StationFromRouteModel>> GetStationsFromTrip(Guid routeID, Guid companyID);
        Task<List<GetStationModel>> GetAllStationActive();
        Task<GetStationModel> GetStationById(Guid id);
        Task<bool> CreateStation(CreateStationModel stationModel);
        Task<string> UpdateStation(Guid stationId, UpdateStationModel stationModel);
        //Task<List<StationFromRouteModel>> GetAllStationFromTrip(Guid id);
        Task<List<GetStationByCompanyModel>> GetAllStationsByCompanyID(Guid companyID);
        Task<List<GetStationModel>> GetStationsByCityId(Guid cityID);
        Task<bool> CompanyRegisStation(Guid companyID, List<Guid> cityID);

    }
}
