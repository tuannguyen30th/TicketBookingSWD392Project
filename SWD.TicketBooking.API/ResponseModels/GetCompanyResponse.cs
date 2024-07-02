using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetCompanyResponse
    {
        [JsonPropertyName("CompanyID")]
        public Guid CompanyID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }
    }
}
