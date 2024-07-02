using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Auth
{
    public class SignUpResponse
    {
        [JsonPropertyName("Verified")]

        public bool? Verified { get; set; }

        [JsonPropertyName("Messages")]
        public string? Messages { get; set; } = string.Empty;
    }
}
