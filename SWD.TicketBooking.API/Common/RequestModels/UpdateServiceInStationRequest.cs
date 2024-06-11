namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class UpdateServiceInStationRequest
    {
        public Guid Station_ServiceID { get; set; }
        public Guid StationID { get; set; }
        public Guid ServiceID { get; set; }
        public double Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
