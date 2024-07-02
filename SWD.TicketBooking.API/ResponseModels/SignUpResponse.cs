using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class SignUpResponse
    {
        [JsonPropertyName("Messages")]
        public string? Messages { get; set; } = string.Empty;
    }
}
