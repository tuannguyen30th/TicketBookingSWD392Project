using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class PriceInSearchTicketModel
    {
        public double price { get; set; }
        public List<StationInSearchTicket> stations { get; set; }
    }
}
