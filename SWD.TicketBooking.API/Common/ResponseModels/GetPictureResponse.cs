namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class GetPictureResponse
    {
        public Guid TripId { get; set; }
        public string? ImageUrl { get; set; }

    }
}
