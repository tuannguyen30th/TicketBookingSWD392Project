namespace SWD.TicketBooking.API.RequestModels
{
    public class UpdateServiceRequest
    {
        public Guid ServiceID { get; set; }
        public Guid ServiceTypeID { get; set; }
        public string? Name { get; set; }

    }
}
