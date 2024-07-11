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
        public Guid CompanyID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;   
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
