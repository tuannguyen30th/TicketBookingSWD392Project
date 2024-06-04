using SWD.TicketBooking.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Auth
{
    public class CheckTokenResponse
    {
        public UserModel User { get; set; } = new UserModel();

    }
}
