using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class DashboardInfoByCompanyModel
    {
        [JsonPropertyName("PopularRoutes")]
        public List<PopularRouteModel> PopularRoutes { get; set; }

        [JsonPropertyName("TotalRoutes")]
        public int TotalRoutes { get; set; }

        [JsonPropertyName("TotalTrips")]
        public int TotalTrips { get; set; }

        [JsonPropertyName("TotalBookingsInMonth")]
        public int TotalBookingsInMonth { get; set; }

        [JsonPropertyName("MonthlyRevenue")]
        public List<MonthlyRevenueModel> MonthlyRevenue { get; set; }

        [JsonPropertyName("YearlyRevenue")]
        public double YearlyRevenue { get; set; }
    }

    public class MonthlyRevenueModel
    {
        [JsonPropertyName("Month")]
        public int Month { get; set; }

        [JsonPropertyName("RevenueInMonth")]
        public double RevenueInMonth { get; set; }
    }
}
