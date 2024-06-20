using SWD.TicketBooking.Service.Dtos.Booking;

namespace SWD.TicketBooking.API.RequestModels.Booking
{
    public class BookingRequest
    {
        public AddOrUpdateBookingRequest AddOrUpdateBookingRequest { get; set; }
        public List<AddOrUpdateTicketRequest> AddOrUpdateTicketRequests { get; set; }
    }
}
