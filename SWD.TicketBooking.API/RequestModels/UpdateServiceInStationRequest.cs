namespace SWD.TicketBooking.API.RequestModels
{
    public class UpdateServiceInStationRequest
    {
        public double Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
