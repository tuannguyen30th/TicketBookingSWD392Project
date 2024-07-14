using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetRouteFromCompanyResponse
    {
        [JsonPropertyName("Route_CompanyID")]
        public Guid Route_CompanyID { get; set; }

        [JsonPropertyName("RouteID")]
        public Guid RouteID { get; set; }

        [JsonPropertyName("FromCity")]
        public string? FromCity { get; set; } = string.Empty;

        [JsonPropertyName("ToCity")]
        public string? ToCity { get; set; } = string.Empty;

        [JsonPropertyName("StartLocation")]
        public string? StartLocation { get; set; } = string.Empty;

        [JsonPropertyName("EndLocation")]
        public string? EndLocation { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public string? Status { get; set; } = string.Empty;
    }
}