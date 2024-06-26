﻿
using System.ComponentModel.DataAnnotations;

namespace SWD.TicketBooking.Service.Dtos.User
{
    public class UserModel
    {
        public Guid UserID { get; set; }

        [MaxLength(255)]
        public string? UserName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Password { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? FullName { get; set; } = string.Empty;

        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; } = string.Empty;

        public string? OTPCode { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? PhoneNumber { get; set; } = string.Empty;
        public double Balance { get; set; }
        public DateTimeOffset? CreateDate { get; set; }
        public bool? IsVerified { get; set; }

        public string? Status { get; set; } = string.Empty;
        public Guid RoleID { get; set; }
    }
}
