using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class DashboardAdminModel
    {
        [JsonPropertyName("TotalRevenueInMonth")]
        public double? TotalRevenueInMonth { get; set; }

        [JsonPropertyName("TotalTicketBookedInMonth")]
        public int? TotalTicketBookedInMonth { get; set; }

        [JsonPropertyName("ToTalUsers")]
        public int? ToTalUsers { get; set; }

        [JsonPropertyName("TotalCompanies")]
        public int? TotalCompanies { get; set; }

        [JsonPropertyName("RevenueAllMonthInYears")]
        public List<RevenueAllMonthInYear?> RevenueAllMonthInYears { get; set; }

        [JsonPropertyName("RevenueOfCompanyInMonths")]
        public List<RevenueOfCompanyInMonth?> RevenueOfCompanyInMonths { get; set; }
    }

    public class RevenueAllMonthInYear
    {
        [JsonPropertyName("Month")]
        public int? Month { get; set; }

        [JsonPropertyName("Year")]
        public int? Year { get; set; }

        [JsonPropertyName("TotalRevenueMonthInYear")]
        public double? TotalRevenueMonthInYear { get; set; }
    }

    public class RevenueOfCompanyInMonth
    {
        [JsonPropertyName("CompanyID")]
        public Guid? CompanyID { get; set; }

        [JsonPropertyName("CompanyName")]
        public string? CompanyName { get; set; }

        [JsonPropertyName("Month")]
        public int? Month { get; set; }

        [JsonPropertyName("Year")]
        public int? Year { get; set; }

        [JsonPropertyName("TotalRevenueOfCompanyInMonth")]
        public double? TotalRevenueOfCompanyInMonth { get; set; }
    }
}