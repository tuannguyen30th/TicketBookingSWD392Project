using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class PopularTripResponse
    {
        public Guid TripId { get; set; }
        public string? FromCity { get; set; }
        public string? ToCity { get; set; }
        public double PriceFrom { get; set; }
        public string? ImageUrl { get; set; }
    }
}
