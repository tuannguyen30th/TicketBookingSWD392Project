using System.ComponentModel.DataAnnotations;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class UserResponse
    {
        public Guid UserID { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Avatar { get; set; }

        public string? Address { get; set; }

        public string? OTPCode { get; set; }

        public string? PhoneNumber { get; set; }
        public double Balance { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public bool? IsVerified { get; set; }
        public string? Status { get; set; }
        public Guid RoleID { get; set; }

    }
}
