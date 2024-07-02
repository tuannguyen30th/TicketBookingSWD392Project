using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class StationFromRouteResponse
    {
        [JsonPropertyName("StationID")]
        public Guid StationID { get; set; }
        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}
