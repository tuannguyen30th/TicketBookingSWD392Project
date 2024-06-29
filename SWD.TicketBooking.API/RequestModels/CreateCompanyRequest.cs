namespace SWD.TicketBooking.API.RequestModels
{
    public class CreateCompanyRequest
    {
        public string? Name { get; set; }
        public Guid UserID { get; set; }

    }
}
