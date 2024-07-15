using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetTripFromCompanyModel
    {
        [JsonPropertyName("TripID")]
        public Guid TripID { get; set; }
        [JsonPropertyName("StaffID")]
        public Guid StaffID { get; set; }
        [JsonPropertyName("FromCity")]
        public string? StaffName { get; set; }
        [JsonPropertyName("MinPrice")]
        public string? FromCity { get; set; }
        [JsonPropertyName("ToCity")]
        public string? ToCity { get; set; }
        [JsonPropertyName("StartLocation")]
        public string? StartLocation { get; set; }
        [JsonPropertyName("EndLocation")]
        public string? EndLocation { get; set; }
        [JsonPropertyName("StartTime")]
        public string? StartTime { get; set; }
        [JsonPropertyName("EndTime")]
        public string? EndTime { get; set; }
        [JsonPropertyName("StartDate")]
        public string? StartDate { get; set; }
        [JsonPropertyName("EndDate")]
        public string? EndDate { get; set; }

        [JsonPropertyName("StaffName")]
        
        public double? MinPrice { get; set; }
        [JsonPropertyName("MaxPrice")]
        public double? MaxPrice { get; set; }
        [JsonPropertyName("Status")]
        public string? Status { get; set; }
    }
}
