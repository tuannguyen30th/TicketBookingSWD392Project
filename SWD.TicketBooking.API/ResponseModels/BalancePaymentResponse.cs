using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class BalancePaymentResponse
    {
        [JsonPropertyName("IsSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }
    }
}
