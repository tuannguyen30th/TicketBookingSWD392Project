namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class GetStationResponse
    {
        public Guid StationID { get; set; }
        public string? Name { get; set; } 
        public string? Status { get; set; } 
    }
}
