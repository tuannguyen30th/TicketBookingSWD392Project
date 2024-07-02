using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetStaffFromCompanyResponse
    {
        [JsonPropertyName("StaffID")]
        public Guid StaffID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
    }
}
