namespace SWD.TicketBooking.API.RequestModels
{
    public class UpdateRouteRequest
    {
        public Guid FromCityID { get; set; }
        public Guid ToCityID { get; set; }
        public string? StartLocation { get; set; }
        public string? EndLocation { get; set; }
    }
}
