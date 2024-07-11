using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetTripDetailsModel
    {
        [JsonPropertyName("TripID")]
        public Guid TripID { get; set; }

        [JsonPropertyName("StaffID")]
        public Guid? StaffID { get; set; }

        [JsonPropertyName("StaffName")]
        public string? StaffName { get; set; }
        [JsonPropertyName("StaffEmail")]
        public string? StaffEmail { get; set; }

        [JsonPropertyName("FromCity")]
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

        [JsonPropertyName("TripPriceSeats")]
        public List<TripPriceSeat> TripPriceSeats { get; set; }

        [JsonPropertyName("ImageUrls")]
        public List<string?> ImageUrls { get; set; }

        [JsonPropertyName("TripUtilityModels")]
        public List<TripUtilityModel?> TripUtilityModels { get; set; }

        [JsonPropertyName("TripStationModels")]
        public List<TripStationModel?> TripStationModels { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        public class TripUtilityModel
        {
            [JsonPropertyName("UtilityName")]
            public string? UtilityName { get; set; }

            [JsonPropertyName("Description")]
            public string? Description { get; set; }
        }

        public class TripStationModel
        {
            [JsonPropertyName("StationID")]
            public Guid? StationID { get; set; }
            [JsonPropertyName("StationName")]
            public string? StationName { get; set; }

            [JsonPropertyName("AtCity")]
            public string? AtCity { get; set; }
        }

        public class TripPriceSeat
        {
            [JsonPropertyName("SeatName")]
            public string? SeatName { get; set; }

            [JsonPropertyName("Price")]
            public double? Price { get; set; }
            [JsonPropertyName("Quantity")]
            public double? Quantity { get; set; }
        }
    }
}