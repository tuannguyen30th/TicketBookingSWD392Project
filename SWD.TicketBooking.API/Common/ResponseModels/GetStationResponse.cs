namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class GetStationResponse
    {
        public int StationID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
