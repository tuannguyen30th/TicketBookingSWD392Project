namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetServiceResponse
    {
        public Guid ServiceID { get; set; }
        public Guid? ServiceTypeID { get; set; }
        public string? Name { get; set; } = string.Empty;
    }
}
