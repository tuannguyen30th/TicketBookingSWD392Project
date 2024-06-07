namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class CreateStationRequest
    {
        public int cityId { get; set; }
        public int companyId { get; set; }
        public string stationName { get; set; }
    }
}
