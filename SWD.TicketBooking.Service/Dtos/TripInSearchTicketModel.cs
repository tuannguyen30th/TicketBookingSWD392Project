using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class TripInSearchTicketModel
    {
        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }

        [JsonPropertyName("Route")]
        public string? Route { get; set; }

        [JsonPropertyName("Company")]
        public string? Company { get; set; }

        [JsonPropertyName("Date")]
        public string? Date { get; set; }

        [JsonPropertyName("Time")]
        public string? Time { get; set; }

        [JsonPropertyName("Position")]
        public string? Position { get; set; }
    }
}