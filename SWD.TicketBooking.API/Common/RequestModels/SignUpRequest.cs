using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class SignUpRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
