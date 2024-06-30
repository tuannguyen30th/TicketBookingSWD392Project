using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Swashbuckle.AspNetCore;

namespace SWD.TicketBooking.API.RequestModels
{
    public class CreateStationWithServiceRequest
    {
        public Guid CompanyID { get; set; }
        public Guid CityID { get; set; }
        public string StationName { get; set; }
        public List<Guid> ServiceIDs { get; set; }
        public List<IFormFile> ServiceImages { get; set; }
        public List<double> Prices { get; set; }
    }
}
