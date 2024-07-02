using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class CitiesResponse
    {
        [JsonPropertyName("CityID")]
        public Guid CityID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public string Status { get; set; } = string.Empty;
    }
}
