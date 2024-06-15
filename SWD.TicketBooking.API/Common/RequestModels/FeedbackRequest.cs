namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class FeedbackRequest
    {
        public Guid UserID { get; set; }
        public Guid TripID { get; set; }
        public int Rating { get; set; }
        public string? Description { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
