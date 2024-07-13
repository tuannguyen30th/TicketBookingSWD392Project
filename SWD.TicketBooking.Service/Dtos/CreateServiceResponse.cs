using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateServiceResponse
    {
        [JsonPropertyName("ServiceID")]
        public Guid ServiceID { get; set; }      
        
        [JsonPropertyName("ServiceTypeID")]
        public Guid ServiceTypeID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public string Status { get; set; } = string.Empty;
    }
}
