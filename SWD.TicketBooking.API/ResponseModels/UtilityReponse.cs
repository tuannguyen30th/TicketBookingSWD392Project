﻿namespace SWD.TicketBooking.API.ResponseModels
{
    public class UtilityReponse
    {
        public Guid UtilityID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
