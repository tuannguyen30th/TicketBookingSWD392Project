using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class DashboardAdminModel
    {
        public double? TotalRevenueInMoth { get; set; }
        public int? TotalTicketBookedInMonth { get; set; }
        public int? ToTalUsers { get; set; }
        public int? TotalCompanies { get; set; }
        public List<RevenueAllMonthInYear?> RevenueAllMonthInYears { get; set; }
        public List<RevenueOfCompanyInMonth?> RevenueOfCompanyInMonths { get; set; }

    }
    public class RevenueAllMonthInYear
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public double? TotalRevenueMonthInYear { get; set; }
    }
    public class RevenueOfCompanyInMonth
    {
        public Guid? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public double? TotalRevenueOfCompanyInMonth { get; set; }
    }
}
