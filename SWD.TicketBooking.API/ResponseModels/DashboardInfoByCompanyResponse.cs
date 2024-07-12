using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class DashboardInfoByCompanyResponse
    {
        [JsonPropertyName("MostPopularRouteID")]
        public Guid MostPopularRouteID { get; set; }

        [JsonPropertyName("MostPopularRoute_FromCity")]
        public string MostPopularRoute_FromCity { get; set; }

        [JsonPropertyName("MostPopularRoute_ToCity")]
        public string MostPopularRoute_ToCity { get; set; }

        [JsonPropertyName("TotalBookingsInPopularRoute")]
        public int TotalBookingsInPopularRoute { get; set; }

        [JsonPropertyName("TotalRoutes")]
        public int TotalRoutes { get; set; }

        [JsonPropertyName("TotalTrips")]
        public int TotalTrips { get; set; }

        [JsonPropertyName("TotalBookingsInMonth")]
        public int TotalBookingsInMonth { get; set; }

        [JsonPropertyName("MonthlyRevenue")]
        public double MonthlyRevenue { get; set; }

        [JsonPropertyName("YearlyRevenue")]
        public double YearlyRevenue { get; set; }
    }
}
