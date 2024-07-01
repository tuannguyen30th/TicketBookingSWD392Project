namespace SWD.TicketBooking.API.RequestModels
{
    public class CreateTripRequest
    {
        public Guid Route_CompanyID { get; set; }
        public bool IsTemplate { get; set; }
        public Guid StaffID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid TemplateID { get; set; }
        public List<IFormFile> ImageUrls { get; set; } = new List<IFormFile>();
        public List<TicketType_TripRequest> TicketType_TripRequests { get; set; } = new List<TicketType_TripRequest>();
        public List<Trip_UtilityRequest> Trip_UtilityRequests { get; set; } = new List<Trip_UtilityRequest>();

        public class TicketType_TripRequest
        {
            public Guid TicketTypeID { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
        }
        public class Trip_UtilityRequest
        {
            public Guid UtilityID { get; set; }
        }
    }
}
