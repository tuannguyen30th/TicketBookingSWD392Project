using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class LoginResponse
    {
        [JsonPropertyName("AccessToken")]
        public string? AccessToken { get; set; } = null!;
    }
}
