using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetSeatBookedFromTripResponse
    {
        [JsonPropertyName("TripID")]
        public Guid TripID { get; set; }

        [JsonPropertyName("RouteID")]
        public Guid RouteID { get; set; }

        [JsonPropertyName("CompanyName")]
        public string? CompanyName { get; set; }

        [JsonPropertyName("StartLocation")]
        public string? StartLocation { get; set; }

        [JsonPropertyName("EndLocation")]
        public string? EndLocation { get; set; }

        [JsonPropertyName("StartDate")]
        public string? StartDate { get; set; }

        [JsonPropertyName("StartTime")]
        public string? StartTime { get; set; }

        [JsonPropertyName("TotalSeats")]
        public int TotalSeats { get; set; }

        [JsonPropertyName("SeatBooked")]
        public List<string> SeatBooked { get; set; } = new List<string>();

        [JsonPropertyName("TicketType_TripResponses")]
        public List<TicketType_TripResponse> TicketType_TripResponses { get; set; }


        public class TicketType_TripResponse
        {
            [JsonPropertyName("TicketType_TripID")]
            public Guid TicketType_TripID { get; set; }

            [JsonPropertyName("TicketName")]
            public string? TicketName { get; set; }

            [JsonPropertyName("Price")]
            public double Price { get; set; }

            [JsonPropertyName("Quantity")]
            public int Quantity { get; set; }
        }
    }
}
