using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class TicketDetail_ServiceRepository : GenericRepository<TicketDetail_Service, Guid>, ITicketDetail_ServiceRepository
    {
        public TicketDetail_ServiceRepository(TicketBookingDbContext context) : base(context) { }
    }
}
