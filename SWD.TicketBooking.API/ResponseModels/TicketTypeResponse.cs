﻿namespace SWD.TicketBooking.API.ResponseModels
{
    public class TicketTypeResponse
    {
        public Guid TicketTypeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
    }
}
