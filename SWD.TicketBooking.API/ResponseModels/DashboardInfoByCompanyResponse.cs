using SWD.TicketBooking.Service.Dtos;
using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class DashboardInfoByCompanyResponse
    {
        [JsonPropertyName("PopularRoutes")]
        public List<PopularRouteResponse> PopularRoutes { get; set; }

        [JsonPropertyName("TotalRoutes")]
        public int TotalRoutes { get; set; }

        [JsonPropertyName("TotalTrips")]
        public int TotalTrips { get; set; }

        [JsonPropertyName("TotalBookingsInMonth")]
        public int TotalBookingsInMonth { get; set; }

        [JsonPropertyName("MonthlyRevenue")]
        public List<MonthlyRevenueResponse> MonthlyRevenue { get; set; }

        [JsonPropertyName("YearlyRevenue")]
        public double YearlyRevenue { get; set; }
    }

    public class MonthlyRevenueResponse
    {
        [JsonPropertyName("Month")]
        public int Month { get; set; }

        [JsonPropertyName("RevenueInMonth")]
        public double RevenueInMonth { get; set; }
    }
}
