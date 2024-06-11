using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetStationModel
    {
        public Guid StationID { get; set; }
        public string? Name { get; set; } 
        public string? Status { get; set; } 
    }
}
