using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class DashboardInfoByCompanyModel
    {
        public Guid MostPopularRouteID { get; set; }
        public string MostPopularRoute_FromCity { get; set; }
        public string MostPopularRoute_ToCity { get; set; }
        public int TotalBookingsInPopularRoute { get; set; }
        public int TotalRoutes { get; set; }
        public int TotalTrips { get; set;}
        public int TotalBookingsInMonth { get; set; }
        public double MonthlyRevenue { get; set; }
        public double YearlyRevenue { get;set; }

    }
}
