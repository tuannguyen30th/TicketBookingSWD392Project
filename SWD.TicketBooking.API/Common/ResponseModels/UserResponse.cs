using System.ComponentModel.DataAnnotations;

namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class UserResponse
    {
        public int UserID { get; set; }

        public string? UserName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;

        public string? FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public string? Address { get; set; } = string.Empty;

        public string? OTPCode { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; } = string.Empty;
        public double Balance { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public bool? IsVerified { get; set; }
        public string? Status { get; set; } = string.Empty;
        public int RoleID { get; set; }

    }
}
