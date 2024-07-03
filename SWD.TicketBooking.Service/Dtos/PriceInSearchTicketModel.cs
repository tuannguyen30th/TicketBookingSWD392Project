using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class PriceInSearchTicketModel
    {
        [JsonPropertyName("Price")]
        public double Price { get; set; }
        [JsonPropertyName("Services")]
        public List<ServiceInSearchTicket> Services { get; set; }
    }
}
