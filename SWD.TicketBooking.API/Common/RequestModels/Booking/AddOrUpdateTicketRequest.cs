using SWD.TicketBooking.Service.Dtos.Booking;

namespace SWD.TicketBooking.API.Common.RequestModels.Booking
{
    public class AddOrUpdateTicketRequest
    {
        public Guid TicketType_TripID { get; set; }
        public double Price { get; set; }
        public string SeatCode { get; set; } = string.Empty;
        public List<AddOrUpdateServiceRequest> AddOrUpdateServiceRequests { get; set; }
    }
}
