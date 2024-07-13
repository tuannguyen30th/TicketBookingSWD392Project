using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class DashboardInfoByCompanyModel
    {
        public List<PopularRouteModel> PopularRoutes { get; set; }
        public int TotalRoutes { get; set; }
        public int TotalTrips { get; set; }
        public int TotalBookingsInMonth { get; set; }
        public List<MonthlyRevenueModel> MonthlyRevenue { get; set; }
        public double YearlyRevenue { get; set; }
    }

    public class MonthlyRevenueModel
    {
        public int Month { get; set; }
        public double RevenueInMonth { get; set; }
    }
}
