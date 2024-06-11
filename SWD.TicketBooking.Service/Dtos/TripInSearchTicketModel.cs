using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class TripInSearchTicketModel
    {
        public string? UserName { get; set; }
        public string? Route { get; set; }
        public string? Company { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Position { get; set; }
    }
}
