namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class FeedbackRequest
    {
        public int UserID { get; set; }
        public int TripID { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<IFormFile> Files { get; set; }
    }
}
