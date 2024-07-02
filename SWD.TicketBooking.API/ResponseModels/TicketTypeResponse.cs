using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class TicketTypeResponse
    {
        [JsonPropertyName("TicketTypeID")]
        public Guid TicketTypeID { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("Status")]
        public string? Status { get; set; } = string.Empty;
    }
}
