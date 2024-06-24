using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class TripRepository : GenericRepository<Trip, Guid>, ITripRepository
    {
        public TripRepository(TicketBookingDbContext context) : base(context) { }
    }
}
