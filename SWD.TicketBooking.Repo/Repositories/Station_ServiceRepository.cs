using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class Station_ServiceRepository : GenericRepository<Station_Service, Guid>, IStation_ServiceRepository
    {
        public Station_ServiceRepository(TicketBookingDbContext context) : base(context) { }
    }
}
