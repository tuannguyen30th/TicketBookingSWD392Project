namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class CreateTripRequest
    {
        public int RouteID { get; set; }
        public bool IsTemplate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<IFormFile> ImageUrls { get; set; } = new List<IFormFile>();
        public List<TicketType_TripRequest> TicketType_TripRequests { get; set; } = new List<TicketType_TripRequest>();
        public List<Trip_UtilityRequest> Trip_UtilityRequests { get; set; } = new List<Trip_UtilityRequest>();

        public class TicketType_TripRequest
        {
            public int TicketTypeID { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
        }
        public class Trip_UtilityRequest
        {
            public int UtilityID { get; set; }
        }
    }
}
