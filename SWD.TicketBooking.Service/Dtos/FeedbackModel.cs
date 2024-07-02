using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class FeedbackModel
    {
        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }

        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("Desciption")]
        public string? Desciption { get; set; }

        [JsonPropertyName("ImageUrl")]
        public List<string> ImageUrl { get; set; } = new List<string>();

        [JsonPropertyName("Rating")]
        public int Rating { get; set; }

        [JsonPropertyName("Avt")]
        public string? Avt { get; set; }
    }
}