using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Auth
{
    public class AccessTokenModel
    {
        [JsonPropertyName("Success")]
        public bool Success { get; set; }

        [JsonPropertyName("Token")]
        public string Token { get; set; }

        [JsonPropertyName("ErrorMessage")]
        public string ErrorMessage { get; set; }
    }
}