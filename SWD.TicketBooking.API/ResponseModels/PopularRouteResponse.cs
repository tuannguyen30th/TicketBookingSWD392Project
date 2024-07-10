using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class PopularRouteResponse
    {
        [JsonPropertyName("RouteID")]
        public Guid RouteID { get; set; }

        [JsonPropertyName("FromCityID")]
        public Guid FromCityID { get; set; }

        [JsonPropertyName("FromCity")]
        public string? FromCity { get; set; }

        [JsonPropertyName("ToCityID")]
        public Guid ToCityID { get; set; }

        [JsonPropertyName("ToCity")]
        public string? ToCity { get; set; }
        [JsonPropertyName("StartLocation")]
        public string StartLocation { get; set; }
        [JsonPropertyName("EndLocation")]
        public string EndLocation { get; set; }
        [JsonPropertyName("TotalBooking")]
        public int TotalBooking { get; set; }
    }
}
