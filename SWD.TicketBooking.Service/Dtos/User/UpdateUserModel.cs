using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.User
{
    public class UpdateUserModel
    {
        public string UserName { get; set; }

        public String confirmPassword { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
