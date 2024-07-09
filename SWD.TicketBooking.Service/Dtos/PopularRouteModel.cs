using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class PopularRouteModel
    {
        public Guid RouteID { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public int TotalBooking { get; set; }
    }
}
