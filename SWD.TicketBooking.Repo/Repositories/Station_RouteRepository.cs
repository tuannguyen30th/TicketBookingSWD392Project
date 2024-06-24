using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class Station_RouteRepository : GenericRepository<Station_Route, Guid>, IStation_RouteRepository
    {
        public Station_RouteRepository(TicketBookingDbContext context) : base(context) { }
    }
}
