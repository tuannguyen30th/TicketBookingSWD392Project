using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class ServiceFromStationModel
    {
        public class ServiceTypeModel
        {
            [JsonPropertyName("ServiceTypeID")]
            public Guid ServiceTypeID { get; set; }

            [JsonPropertyName("StationID")]
            public Guid StationID { get; set; }

            [JsonPropertyName("Name")]
            public string? Name { get; set; }

            [JsonPropertyName("ServiceModels")]
            public List<ServiceModel> ServiceModels { get; set; }
        }
        public class ServiceModel
        {
            [JsonPropertyName("ServiceID")]
            public Guid ServiceID { get; set; }

            [JsonPropertyName("Name")]
            public string? Name { get; set; }

            [JsonPropertyName("Price")]
            public double Price { get; set; }

            [JsonPropertyName("ImageUrl")]
            public string? ImageUrl { get; set; }
        }
    }
}
