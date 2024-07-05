using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class Station_CompanyRepository : GenericRepository<Station_Company, Guid>, IStation_CompanyRepository
    {
        public Station_CompanyRepository(TicketBookingDbContext context) : base(context) { }
    }
}
