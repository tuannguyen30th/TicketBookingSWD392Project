using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; } = null!;
    }
}
