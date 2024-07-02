using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class UtilityReponse
    {
        [JsonPropertyName("UtilityID")]
        public Guid UtilityID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;
    }
}
