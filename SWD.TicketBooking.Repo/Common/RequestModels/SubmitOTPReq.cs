using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Common.RequestModels
{
    public class SubmitOTPReq
    {
        public string Email { get; set; }
        public string OTPCode { get; set; }
    }
}
