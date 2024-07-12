using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetCompanyAfterCreateModel
    {
        [JsonPropertyName("StationID")]
        public Guid StationID { get; set; }

        [JsonPropertyName("CityID")]
        public Guid CityID { get; set; }

        [JsonPropertyName("CityName")]
        public string? CityName { get; set; }

        [JsonPropertyName("StationName")]
        public string? StationName { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }
    }
}