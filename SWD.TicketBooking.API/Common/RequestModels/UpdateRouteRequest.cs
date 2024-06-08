namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class UpdateRouteRequest
    {
        public int FromCityID { get; set; }
        public int ToCityID { get; set; }
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
    }
}
