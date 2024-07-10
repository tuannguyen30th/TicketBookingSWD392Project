using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class ServiceInSearchTicket
    {
        [JsonPropertyName("ServiceName")]
        public string? ServiceName { get; set; }
        [JsonPropertyName("Price")]
        public double Price { get; set; }
        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }

    }
}
