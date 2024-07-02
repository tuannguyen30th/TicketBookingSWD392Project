using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{

    public class GetTicketDetailInMobileResponse
    {
        [JsonPropertyName("QrCodeImage")]
        public string QrCodeImage { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("StartTime")]
        public string StartTime { get; set; }

        [JsonPropertyName("StartDay")]
        public string StartDay { get; set; }

        [JsonPropertyName("SeatCode")]
        public string SeatCode { get; set; }

        [JsonPropertyName("Route")]
        public string Route { get; set; }

        [JsonPropertyName("Services")]
        public List<ServiceInTicketResponse> Services { get; set; } = new List<ServiceInTicketResponse>();
    }

    public class ServiceInTicketResponse
    {
        [JsonPropertyName("ServiceName")]
        public string ServiceName { get; set; }

        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("Station")]
        public string Station { get; set; }

        [JsonPropertyName("TotalPrice")]
        public double TotalPrice { get; set; }

    }
}
