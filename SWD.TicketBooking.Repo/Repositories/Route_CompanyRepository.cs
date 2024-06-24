using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class Route_CompanyRepository : GenericRepository<Route_Company, Guid>, IRoute_CompanyRepository
    {
        public Route_CompanyRepository(TicketBookingDbContext context) : base(context) { }
    }
}
