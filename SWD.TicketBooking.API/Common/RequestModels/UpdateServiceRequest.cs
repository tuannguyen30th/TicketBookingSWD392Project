namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class UpdateServiceRequest
    {
        public int ServiceID { get; set; }
        public int ServiceTypeID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
