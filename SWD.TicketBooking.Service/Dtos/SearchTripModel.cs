using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class SearchTripModel
    {
        public int TripID { get; set; }
        public string CompanyName { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public int QuantityRating { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EmptySeat { get; set; }
        public double Price { get; set; }
    }
}
