using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class PopularTripModel
    {
        public Guid TripId { get; set; }
        public string? FromCity { get; set; }
        public string? ToCity { get; set; }
        public double PriceFrom { get; set; }
        public string? ImageUrl { get; set; }
    }
}
