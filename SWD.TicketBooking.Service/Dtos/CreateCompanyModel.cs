using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateCompanyModel
    {
        public string? Name { get; set; }
        public Guid UserID { get; set; }
    }
}
