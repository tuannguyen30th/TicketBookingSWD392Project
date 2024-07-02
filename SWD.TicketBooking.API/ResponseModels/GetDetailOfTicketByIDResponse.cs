using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetDetailOfTicketByIDResponse
    {
        [JsonPropertyName("BookingID")]
        public Guid BookingID { get; set; }

        [JsonPropertyName("CustomerName")]
        public string? CustomerName { get; set; }

        [JsonPropertyName("CompanyName")]
        public string? CompanyName { get; set; }

        [JsonPropertyName("StartDate")]
        public string? StartDate { get; set; }

        [JsonPropertyName("StartTime")]
        public string? StartTime { get; set; }

        [JsonPropertyName("EndDate")]
        public string? EndDate { get; set; }

        [JsonPropertyName("EndTime")]
        public string? EndTime { get; set; }

        [JsonPropertyName("StartCity")]
        public string? StartCity { get; set; }

        [JsonPropertyName("EndCity")]
        public string? EndCity { get; set; }

        [JsonPropertyName("SeatCode")]
        public string? SeatCode { get; set; }

        [JsonPropertyName("TicketPrice")]
        public double TicketPrice { get; set; }

        [JsonPropertyName("TotalServicePrice")]
        public double TotalServicePrice { get; set; }

        [JsonPropertyName("SumOfPrice")]
        public double SumOfPrice { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("QrCodeImage")]
        public string? QrCodeImage { get; set; }

        [JsonPropertyName("QrCode")]
        public string? QrCode { get; set; }

        [JsonPropertyName("ServiceDetailList")]
        public List<ServiceDetailResponse> ServiceDetailList { get; set; }
    }

    public class ServiceDetailResponse
    {
        [JsonPropertyName("ServiceName")]
        public string? ServiceName { get; set; }

        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("ServicePrice")]
        public double ServicePrice { get; set; }

        [JsonPropertyName("ServiceInStation")]
        public string? ServiceInStation { get; set; }
    }

}
