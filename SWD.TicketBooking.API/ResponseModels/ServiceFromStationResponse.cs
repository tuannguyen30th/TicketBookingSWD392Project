using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class ServiceFromStationResponse
    {
        public class ServiceTypeResponse
        {
            public Guid ServiceTypeID { get; set; }
            public Guid StationID { get; set; }
            public string? Name { get; set; }

            public List<ServiceModel> ServiceModels { get; set; }
        }
        public class ServiceResponse
        {
            public Guid ServiceID { get; set; }
            public string? Name { get; set; }
            public double Price { get; set; }
            public string? ImageUrl { get; set; }
        }
    }
}
