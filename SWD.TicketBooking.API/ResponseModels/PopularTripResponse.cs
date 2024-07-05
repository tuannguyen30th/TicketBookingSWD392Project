using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class PopularTripResponse
    {
        [JsonPropertyName("TripId")]
        public Guid TripId { get; set; }

        [JsonPropertyName("FromCityId")]
        public Guid FromCityId { get; set; }

        [JsonPropertyName("FromCity")]
        public string? FromCity { get; set; }

        [JsonPropertyName("ToCityId")]
        public Guid ToCityId { get; set; }

        [JsonPropertyName("ToCity")]
        public string? ToCity { get; set; }
        [JsonPropertyName("StartTime")]
        public DateTime? StartTime { get; set; }

        [JsonPropertyName("PriceFrom")]
        public double PriceFrom { get; set; }

        [JsonPropertyName("ImageUrl")]
        public List<string>? ImageUrl { get; set; }
    }
}
