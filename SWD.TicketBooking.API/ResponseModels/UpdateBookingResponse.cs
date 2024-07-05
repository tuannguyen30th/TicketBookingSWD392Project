using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class UpdateBookingResponse
    {
        [JsonPropertyName ("RspCode")]
        public string RspCode { get; set; }
        [JsonPropertyName("Message")]
        public string Message { get; set; }
    }
}
