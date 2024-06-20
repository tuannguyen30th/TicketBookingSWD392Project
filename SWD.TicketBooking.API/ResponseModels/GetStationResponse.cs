namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetStationResponse
    {
        public Guid StationID { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
    }
}
