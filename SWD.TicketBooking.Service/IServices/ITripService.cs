using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface ITripService
    {
        Task<List<string>> GetPictureOfTrip(Guid id);
        Task<List<PopularTripModel>> GetPopularTrips();
        Task<PagedResult<SearchTripModel>> SearchTrip(Guid fromCity, Guid toCity, DateTime startTime, int pageNumber, int pageSize);
        Task<bool> CreateTrip(CreateTripModel createTrip);
        Task<bool> ChangeStatusTrip(Guid tripId);
        Task<GetSeatBookedFromTripModel> GetSeatBookedFromTrip(Guid tripID);
        Task<List<UtilityModel>> GetAllUtilityByTripID(Guid id);
    }
}
