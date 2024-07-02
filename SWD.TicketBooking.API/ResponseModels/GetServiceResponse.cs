using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetServiceResponse
    {
        [JsonPropertyName("ServiceID")]
        public Guid ServiceID { get; set; }

        [JsonPropertyName("ServiceTypeID")]
        public Guid? ServiceTypeID { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; } = string.Empty;
    }
}
