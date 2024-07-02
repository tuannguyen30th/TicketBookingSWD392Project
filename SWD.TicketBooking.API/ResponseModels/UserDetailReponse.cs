using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class UserDetailReponse
    {
        [JsonPropertyName("UserID")]
        public Guid UserID { get; set; }

        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }

        [JsonPropertyName("Password")]
        public string? Password { get; set; }

        [JsonPropertyName("FullName")]
        public string? FullName { get; set; }

        [JsonPropertyName("Email")]
        public string? Email { get; set; }

        [JsonPropertyName("Avatar")]
        public string? Avatar { get; set; }

        [JsonPropertyName("Address")]
        public string? Address { get; set; }

        [JsonPropertyName("OTPCode")]
        public string? OTPCode { get; set; }

        [JsonPropertyName("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("Balance")]
        public double Balance { get; set; }

        [JsonPropertyName("CreateDate")]
        public DateTimeOffset? CreateDate { get; set; }

        [JsonPropertyName("IsVerified")]
        public bool? IsVerified { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("RoleID")]
        public Guid RoleID { get; set; }

        [JsonPropertyName("CompanyID")]
        public Guid CompanyID { get; set; }

        [JsonPropertyName("RoleName")]
        public string RoleName { get; set; }

    }
}
