namespace SWD.TicketBooking.API.RequestModels
{
    public class CreateServiceRequest
    {
        public Guid ServiceTypeID { get; set; }
        public string? Name { get; set; }

    }
}
