using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class PagedResultResponse<T>
    {
        [JsonPropertyName("Items")]
        public List<T> Items { get; set; }

        [JsonPropertyName("TotalCount")]
        public int TotalCount { get; set; }
    }
}
