using System.Text.Json.Serialization;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetServiceTypeModel
    {
        [JsonPropertyName("ServiceTypeID")]
        public Guid ServiceTypeID { get; set; }

        [JsonPropertyName("ServiceTypeName")]
        public string ServiceTypeName { get; set; } = string.Empty;

        [JsonPropertyName("Services")]
        public List<GetServiceModel> Services { get; set; }
    }

    public class GetServiceModel
    {
        [JsonPropertyName("ServiceID")]
        public Guid ServiceID { get; set; }

        [JsonPropertyName("ServiceName")]
        public string ServiceName { get; set; } = string.Empty;
    }
}
