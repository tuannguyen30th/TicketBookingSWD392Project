using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class CityRepository : GenericRepository<City, Guid>, ICityRepository
    {
        public CityRepository(TicketBookingDbContext context) : base(context) { }
    }
}
