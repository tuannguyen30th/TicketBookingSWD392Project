using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.IRepositories
{
    public interface ITrip_UtilityRepository : IRepository<Trip_Utility, Guid>
    {
    }
}
