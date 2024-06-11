using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class TripInSearchTicketModel
    {
        public string userName { get; set; }
        public string route { get; set; }
        public string company { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string position { get; set; }
    }
}
