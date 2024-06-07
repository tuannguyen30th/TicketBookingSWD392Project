namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class CreateServiceRequest
    {
        public int ServiceTypeID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
