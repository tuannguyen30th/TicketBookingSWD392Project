using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Auth
{
    public class LoginResponse
    {
        public bool Authenticated { get; set; }
        public SecurityToken? Token { get; set; }
    }
}
