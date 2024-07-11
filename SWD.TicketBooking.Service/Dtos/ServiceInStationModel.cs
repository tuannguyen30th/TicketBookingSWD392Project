using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class ServiceTypeInStationModel
    {
        [JsonPropertyName("ServiceTypeID")]
        public Guid ServiceTypeID { get; set; }
        [JsonPropertyName("ServiceTypeName")]
        public string ServiceTypeName { get; set; }
        [JsonPropertyName("ServiceInStation")]
        public List<ServiceInStationModel> ServiceInStation { get; set; }
    }

    public class ServiceInStationModel
    {
        [JsonPropertyName("Service_StationID")]
        public Guid Service_StationID { get; set; }
        [JsonPropertyName("ServiceID")]
        public Guid ServiceID { get; set; }
        [JsonPropertyName("Price")]
        public double Price { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("ImageUrl")]
        public string ImageUrl { get; set; }
    }
}
