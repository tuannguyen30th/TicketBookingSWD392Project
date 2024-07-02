using System.Text.Json.Serialization;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class ServiceFromStationResponse
    {
        public class ServiceTypeResponse
        {
            [JsonPropertyName("ServiceTypeID")]
            public Guid ServiceTypeID { get; set; }

            [JsonPropertyName("StationID")]
            public Guid StationID { get; set; }

            [JsonPropertyName("Name")]
            public string? Name { get; set; }

            [JsonPropertyName("ServiceModels")]
            public List<ServiceModel> ServiceModels { get; set; }
        }
        public class ServiceResponse
        {
            [JsonPropertyName("ServiceID")]
            public Guid ServiceID { get; set; }

            [JsonPropertyName("Name")]
            public string? Name { get; set; }

            [JsonPropertyName("Price")]
            public double Price { get; set; }

            [JsonPropertyName("ImageUrl")]
            public string? ImageUrl { get; set; }
        }
    }
}
