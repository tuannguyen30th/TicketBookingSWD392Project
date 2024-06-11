namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class CreateServiceRequest
    {
        public Guid ServiceTypeID { get; set; }
        public string? Name { get; set; }
      
    }
}
