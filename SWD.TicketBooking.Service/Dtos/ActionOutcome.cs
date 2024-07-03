using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class ActionOutcome
    {
        [JsonPropertyName("Result")]
        public object? Result { get; set; }
        [JsonPropertyName("IsSuccess")]
        public bool IsSuccess { get; set; } = true;
        [JsonPropertyName("Message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("Value")]
        public string Value { get; set; } = string.Empty;
    }
}
