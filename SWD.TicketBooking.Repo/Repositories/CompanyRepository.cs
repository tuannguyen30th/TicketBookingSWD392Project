using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class CompanyRepository : GenericRepository<Company, Guid>, ICompanyRepository
    {
        public CompanyRepository(TicketBookingDbContext context) : base(context) { }
    }
}
