﻿namespace SWD.TicketBooking.API.RequestModels.Booking
{
    public class AddOrUpdateServiceRequest
    {
        public Guid ServiceID { get; set; }
        public Guid StationID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
