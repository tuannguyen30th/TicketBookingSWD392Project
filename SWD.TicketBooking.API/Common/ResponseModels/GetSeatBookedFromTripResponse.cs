namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class GetSeatBookedFromTripResponse
    {
        public int TripID { get; set; }
        public int RouteID { get; set; }
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public List<string> SeatBooked { get; set; } = new List<string>();
        public List<TicketType_TripResponse> TicketType_TripResponses { get; set; }


        public class TicketType_TripResponse
        {
            public int TicketType_TripID { get; set; }
            public string TicketName { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}
