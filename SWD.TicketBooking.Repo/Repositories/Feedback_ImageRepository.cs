using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Repositories
{
    public class Feedback_ImageRepository : GenericRepository<Feedback_Image, Guid>, IFeedback_ImageRepository
    {
        public Feedback_ImageRepository(TicketBookingDbContext context) : base(context) { }
    }
}
