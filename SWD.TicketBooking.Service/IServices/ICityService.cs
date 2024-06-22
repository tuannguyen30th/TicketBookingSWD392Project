using SWD.TicketBooking.Service.Dtos;
using static SWD.TicketBooking.Service.Dtos.FromCityToCityModel;

namespace SWD.TicketBooking.Service.IServices
{
    public interface ICityService
    {
        Task<List<CitiesModel>> GetAllCities();
        Task<CityModel> GetFromCityToCity();
        Task<int> CreateCity(CreateCityModel model);
        Task<int> UpdateCity(Guid cityId, CreateCityModel model);
        Task<int> ChangeStatus(Guid cityId, string status);
    }
}
