using Microsoft.AspNetCore.Mvc;

namespace SWD.TicketBooking.API.RequestModels
{
    public class UpdateBookingRequest
    {
        public Guid BookingId {  get; set; }
            
        public string VnPayResponseCode { get; set; }
    }
}
