using System.Text.Json.Serialization;

namespace SWD.TicketBooking.Service.Dtos.User
{
    public class UpdateUserResponseModel
    {
        [JsonPropertyName("UserName")]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("FullName")]
        public string FullName { get; set; }

        [JsonPropertyName("Avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("Address")]
        public string Address { get; set; }

        [JsonPropertyName("PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}