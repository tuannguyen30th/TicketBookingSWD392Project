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
        public Guid FromCityID { get; set; }
        public string? FromCity { get; set; }
        public Guid ToCityID { get; set; }
        public string? ToCity { get; set; }
        public double PriceFrom { get; set; }
        public List<string>? ImageUrl { get; set; }
    }
}
