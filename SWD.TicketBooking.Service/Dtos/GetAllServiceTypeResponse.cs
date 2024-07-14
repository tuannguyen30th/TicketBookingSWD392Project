using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetAllServiceTypeResponse
    {
        [JsonPropertyName("ServiceTypeID")]
        public Guid ServiceTypeID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
    }
}
