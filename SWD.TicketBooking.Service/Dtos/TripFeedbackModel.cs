using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class TripFeedbackModel
    {
        public List<FeedbackModel> Feedbacks { get; set; }
        public double TotalRating { get; set; }
    }
}
