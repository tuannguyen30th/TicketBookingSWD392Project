using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetRouteFromCompanyModel
    {
        public Guid Route_CompanyID { get; set; }
        public string? FromCity { get; set; } = string.Empty;
        public string? ToCity { get; set; } = string.Empty;
        public string? StartLocation { get; set; } = string.Empty;
        public string? EndLocation { get; set; } = string.Empty;
      
    }
}
