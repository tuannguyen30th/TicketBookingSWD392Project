using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class UpdateRouteModel
    {
        public Guid FromCityID { get; set; }
        public Guid ToCityID { get; set; }
        public string? StartLocation { get; set; } 
        public string? EndLocation { get; set; } 
    }                
}
