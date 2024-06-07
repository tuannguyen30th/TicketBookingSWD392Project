namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class CreateTripRequest
    {
        public int RouteID { get; set; }
        public bool IsTemplate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<IFormFile> ImageUrls { get; set; }
    }
}
