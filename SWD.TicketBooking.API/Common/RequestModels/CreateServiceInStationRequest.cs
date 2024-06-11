﻿namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class CreateServiceInStationRequest
    {
        public Guid StationID { get; set; }
        public Guid ServiceID { get; set; }
        public double Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
