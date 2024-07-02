using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetSeatBookedFromTripModel
    {
        [JsonPropertyName("TripID")]
        public Guid TripID { get; set; }

        [JsonPropertyName("RouteID")]
        public Guid RouteID { get; set; }

        [JsonPropertyName("CompanyName")]
        public string? CompanyName { get; set; }

        [JsonPropertyName("StartLocation")]
        public string? StartLocation { get; set; }

        [JsonPropertyName("EndLocation")]
        public string? EndLocation { get; set; }

        [JsonPropertyName("StartDate")]
        public string? StartDate { get; set; }

        [JsonPropertyName("StartTime")]
        public string? StartTime { get; set; }

        [JsonPropertyName("TotalSeats")]
        public int TotalSeats { get; set; }

        [JsonPropertyName("SeatBooked")]
        public List<string> SeatBooked { get; set; } = new List<string>();

        [JsonPropertyName("TicketType_TripModels")]
        public List<TicketType_TripModel> TicketType_TripModels { get; set; }


        public class TicketType_TripModel
        {
            [JsonPropertyName("TicketType_TripID")]
            public Guid TicketType_TripID { get; set; }

            [JsonPropertyName("TicketName")]
            public string? TicketName { get; set; }

            [JsonPropertyName("Quantity")]
            public int Quantity { get; set; }

            [JsonPropertyName("Price")]
            public double Price { get; set; }
        }

    }
}
