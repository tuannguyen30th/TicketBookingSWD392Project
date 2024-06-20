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
        Task<ActionOutcome> GetFromCityToCity();
        Task<ActionOutcome> CreateCity(CreateCityModel model);
        Task<ActionOutcome> UpdateCity(Guid cityId, CreateCityModel model);
        Task<ActionOutcome> ChangeStatus(Guid cityId, string status);
    }
}
