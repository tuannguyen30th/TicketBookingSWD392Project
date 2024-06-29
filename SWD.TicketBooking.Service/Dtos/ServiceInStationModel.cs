using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class ServicesInStationResponse
    {
        public Guid ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; } 
        public List<ServiceInStationModel> ServiceInStation { get; set; }
    }

    public class ServiceInStationModel
    {
        public Guid ServiceID { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
