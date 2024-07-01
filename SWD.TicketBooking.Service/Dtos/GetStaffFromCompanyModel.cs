using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetStaffFromCompanyModel
    {
        public Guid StaffID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
