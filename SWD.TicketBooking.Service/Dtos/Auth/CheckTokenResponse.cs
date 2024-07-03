using SWD.TicketBooking.Service.Dtos.User;
using System.Text.Json.Serialization;
namespace SWD.TicketBooking.Service.Dtos.Auth
{
    public class CheckTokenResponse
    {
        [JsonPropertyName("User")]
        public UserModel User { get; set; } = new UserModel();

        [JsonPropertyName("RoleName")]
        public string? RoleName { get; set; } = string.Empty;
    }
}
