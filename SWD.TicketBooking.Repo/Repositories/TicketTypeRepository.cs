using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class TicketTypeRepository : GenericRepository<TicketType, Guid>, ITicketTypeRepository
    {
        public TicketTypeRepository(TicketBookingDbContext context) : base(context) { }
    }
}
