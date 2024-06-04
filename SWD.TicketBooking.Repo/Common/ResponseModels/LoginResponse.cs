using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Common.ResponseModels
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
    }
}
