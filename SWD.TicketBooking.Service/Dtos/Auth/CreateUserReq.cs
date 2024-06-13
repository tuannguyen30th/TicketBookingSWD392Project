using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Auth
{
    public class CreateUserReq
    {
        public Guid UserID { get; set; }
        public string? UserName { get; set; }


        public string? Password { get; set; }


        public string? FullName { get; set; }

        public string Email { get; set; } = string.Empty;
        public string? Avatar { get; set; }

        public string? Address { get; set; }

        public string? OTPCode { get; set; }

        public string? PhoneNumber { get; set; }

        public string? FSU { get; set; }
        public string? CreateBy { get; set; }

        public DateTimeOffset? CreateDate { get; set; }

        public string? ModifyBy { get; set; }

        public DateTimeOffset? ModifyDate { get; set; }
        public bool? IsVerified { get; set; }

        public string? Status { get; set; }
        public Guid RoleID { get; set; }
    }
}
