﻿namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class GetTicketDetailByUserResponse
    {
        public int BookingID { get; set; }
        public int TicketDetailID { get; set; }
        public string CompanyName { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public string SeatCode { get; set; }
        public double TicketPrice { get; set; }
        public double TotalServicePrice { get; set; }
        public string Status { get; set; }

    }
}
