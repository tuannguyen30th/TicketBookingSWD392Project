namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class FeedbackRequest
    {
        public Guid UserID { get; set; }
        public Guid TripID { get; set; }
        public Guid Rating { get; set; }
        public string? Description { get; set; } 
        public string? Status { get; set; } 
        public List<IFormFile> Files { get; set; }
    }
}
