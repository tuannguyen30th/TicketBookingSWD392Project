using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class SendMailBookingResponse
    {
        public class MailBookingResponse
        {
            [JsonPropertyName("Price")]
            public double Price { get; set; }

            [JsonPropertyName("FullName")]
            public string? FullName { get; set; }

            [JsonPropertyName("FromTo")]
            public string? FromTo { get; set; }

            [JsonPropertyName("StartTime")]
            public string? StartTime { get; set; }

            [JsonPropertyName("StartDate")]
            public string? StartDate { get; set; }

            [JsonPropertyName("SeatCode")]
            public string? SeatCode { get; set; }

            [JsonPropertyName("TotalBill")]
            public string? TotalBill { get; set; }

            [JsonPropertyName("QrCodeImage")]
            public string? QrCodeImage { get; set; }

            [JsonPropertyName("MailBookingServices")]
            public List<MailBookingServiceResponse> MailBookingServices { get; set; }
        }
        public class MailBookingServiceResponse
        {
            [JsonPropertyName("ServicePrice")]
            public double ServicePrice { get; set; }

            [JsonPropertyName("AtStation")]
            public string AtStation { get; set; }
        }
    }
}
