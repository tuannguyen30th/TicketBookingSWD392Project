namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class ServiceFromStationResponse
    {
        public class ServiceTypeResponse
        {
            public int ServiceTypeID { get; set; }
            public string Name { get; set; }
            public List<ServiceResponse> ServiceResponses { get; set; }
        }
        public class ServiceResponse
        {
            public int ServiceID { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
        }
    }
}
