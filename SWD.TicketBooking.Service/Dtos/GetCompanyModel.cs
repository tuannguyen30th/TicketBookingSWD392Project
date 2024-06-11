using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetCompanyModel
    {
        public Guid CompanyID { get; set; }
        public string? Name { get; set; } 
        public string? Status { get; set; } 
    }
}
