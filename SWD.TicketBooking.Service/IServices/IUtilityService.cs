using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IUtilityService
    {
        Task<List<UtilityModel>> GetAllUtilityByTripID(Guid id);
        Task<List<Utility>> GetAllUtility();
        Task<int> CreateNewUtility(CreateUtilityModel utility);

    }
}
