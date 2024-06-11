using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class PriceInSearchTicketModel
    {
        public double Price { get; set; }
        public List<StationInSearchTicket> Stations { get; set; }
    }
}
