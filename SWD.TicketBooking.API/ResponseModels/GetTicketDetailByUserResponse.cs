using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetTicketDetailByUserResponse
    {
        [JsonPropertyName("BookingID")]
        public Guid BookingID { get; set; }

        [JsonPropertyName("TicketDetailID")]
        public Guid TicketDetailID { get; set; }

        [JsonPropertyName("UserID")]
        public Guid UserID { get; set; }

        [JsonPropertyName("TripID")]
        public Guid TripID { get; set; }

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

        [JsonPropertyName("TotalTime")]
        public TimeSpan TotalTime { get; set; }

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

        [JsonPropertyName("Status")]
        public string? Status { get; set; }        
        [JsonPropertyName("IsRated")]
        public bool? IsRated { get; set; }

    }
}
