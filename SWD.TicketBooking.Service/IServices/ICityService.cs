using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.BackendService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
