using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class ServiceFromStationModel
    {
        public class ServiceTypeModel
        {
            public Guid ServiceTypeID { get; set; }
            public Guid StationID { get; set; }
            public string? Name { get; set; }
         
            public List<ServiceModel> ServiceModels { get; set; }
        }
        public class ServiceModel
        {
            public Guid ServiceID { get; set; }         
            public string? Name { get; set; }
            public double Price { get; set; }
            public string? ImageUrl { get; set; }
        }
    }
}
