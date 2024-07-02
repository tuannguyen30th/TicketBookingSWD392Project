using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetStationResponse
    {
        [JsonPropertyName("StationID")]
        public Guid StationID { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }
    }
}
