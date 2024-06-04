
using System.ComponentModel.DataAnnotations;

namespace SWD.TicketBooking.Service.Dtos.User
{
    public class UserModel
    {
        public int UserID { get; set; }

        [MaxLength(255)]
        public string? UserName { get; set; }

        [MaxLength(100)]
        public string? Password { get; set; }

        [MaxLength(50)]
        public string? FullName { get; set; }

        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }
        public string? Avatar { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public string? OTPCode { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public string? FSU { get; set; }
        public string? CreateBy { get; set; }

        public DateTimeOffset? CreateDate { get; set; }

        public string? ModifyBy { get; set; }

        public DateTimeOffset? ModifyDate { get; set; }
        public bool? IsVerified { get; set; }

        public string? Status { get; set; }
        public int RoleID { get; set; }
    }
}
