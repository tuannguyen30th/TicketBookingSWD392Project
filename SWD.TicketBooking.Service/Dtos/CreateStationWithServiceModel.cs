using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateStationWithServiceModel
    {
        public Guid CompanyID { get; set; }
        public Guid CityID { get; set; }
        public string StationName { get; set; }
        public List<ServiceToCreateModel> ServiceToCreateModels { get; set; }
    }

    public class ServiceToCreateModel
    {
        public Guid ServiceID { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
    }
}
