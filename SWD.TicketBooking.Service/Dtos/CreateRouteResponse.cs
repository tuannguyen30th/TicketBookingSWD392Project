using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateRouteResponse
    {
        [JsonPropertyName("RouteID")]
        public Guid RouteID { get; set; }
        [JsonPropertyName("Route_CompanyID")]
        public Guid Route_CompanyID { get; set; }
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
