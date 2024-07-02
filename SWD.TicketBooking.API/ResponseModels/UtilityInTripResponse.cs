using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class UtilityInTripResponse
    {

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }
    }
}
