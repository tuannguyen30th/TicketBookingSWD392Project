using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class AddServiceToStationModel
    {
        public Guid? StationID { get; set; }
        public List<ServiceToCreateModel>? ServiceToCreateModels { get; set; }
    }

    public class ServiceToCreateModel
    {
        public Guid? ServiceTypeID { get; set; }
        public Guid? ServiceID { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public IFormFile? Image { get; set; }
    }
}
