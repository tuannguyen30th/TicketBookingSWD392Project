namespace SWD.TicketBooking.API.RequestModels
{
    public class CreateStationRequest
    {
        public Guid CityId { get; set; }
        public Guid CompanyId { get; set; }
        public string? StationName { get; set; }
        public Guid RouteId { get; set; }

        public int OrderInRoute { get; set; }

    }
}
