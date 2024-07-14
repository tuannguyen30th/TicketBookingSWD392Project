using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Swashbuckle.AspNetCore;

namespace SWD.TicketBooking.API.RequestModels
{
    public class AddServiceToStationRequest
    {
        public Guid? StationID { get; set; }
        public List<ServiceToCreateRequest> ServicesToCreate { get; set; }
        public List<IFormFile>? ServiceImages { get; set; }
    }

    public class ServiceToCreateRequest
    {
        public Guid? ServiceTypeID { get; set; }
        public Guid? ServiceID { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
    }
}
