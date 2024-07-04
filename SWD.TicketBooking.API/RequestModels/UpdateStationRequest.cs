namespace SWD.TicketBooking.API.RequestModels
{
    public class UpdateStationRequest
    {
        public Guid CityId { get; set; }
        public Guid CompanyId { get; set; }
        public string? StationName { get; set; }
    }
}
