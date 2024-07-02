using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class SearchTripResponse
    {
        [JsonPropertyName("TripID")]
        public Guid TripID { get; set; }

        [JsonPropertyName("RouteID")]
        public Guid RouteID { get; set; }

        [JsonPropertyName("TemplateID")]
        public Guid TemplateID { get; set; }

        [JsonPropertyName("CompanyName")]
        public string? CompanyName { get; set; }

        [JsonPropertyName("ImageUrl")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("AverageRating")]
        public double AverageRating { get; set; }

        [JsonPropertyName("QuantityRating")]
        public int QuantityRating { get; set; }

        [JsonPropertyName("StartLocation")]
        public string? StartLocation { get; set; }

        [JsonPropertyName("EndLocation")]
        public string? EndLocation { get; set; }

        [JsonPropertyName("StartDate")]
        public string? StartDate { get; set; }

        [JsonPropertyName("EndDate")]
        public string? EndDate { get; set; }

        [JsonPropertyName("StartTime")]
        public string? StartTime { get; set; }

        [JsonPropertyName("EndTime")]
        public string? EndTime { get; set; }

        [JsonPropertyName("EmptySeat")]
        public int EmptySeat { get; set; }

        [JsonPropertyName("Price")]
        public double Price { get; set; }
    }
}
