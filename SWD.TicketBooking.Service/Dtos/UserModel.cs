
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWD.TicketBooking.Service.Dtos.User
{
    public class UserModel
    {
        [JsonPropertyName("UserID")]

        public Guid UserID { get; set; }

        [MaxLength(255)]
        [JsonPropertyName("UserName")]
        public string? UserName { get; set; } = string.Empty;

        [MaxLength(100)]
        [JsonPropertyName("Password")]
        public string? Password { get; set; } = string.Empty;

        [MaxLength(50)]
        [JsonPropertyName("FullName")]
        public string? FullName { get; set; } = string.Empty;

        [MaxLength(255)]
        [EmailAddress]
        [JsonPropertyName("Email")]
        public string? Email { get; set; } = string.Empty;

        [JsonPropertyName("Avatar")]
        public string? Avatar { get; set; }

        [MaxLength(255)]
        [JsonPropertyName("Address")]
        public string? Address { get; set; } = string.Empty;

        [JsonPropertyName("OTPCode")]
        public string? OTPCode { get; set; } = string.Empty;

        [MaxLength(15)]
        [JsonPropertyName("PhoneNumber")]
        public string? PhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("Balance")]
        public double? Balance { get; set; }

        [JsonPropertyName("CreateDate")]
        public DateTimeOffset? CreateDate { get; set; }

        [JsonPropertyName("IsVerified")]
        public bool? IsVerified { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; } = string.Empty;

        [JsonPropertyName("CompanyID")]
        public Guid? CompanyID { get; set; }

        [JsonPropertyName("RoleID")]
        public Guid? RoleID { get; set; }
    }
}
