namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class GetSeatBookedFromTripResponse
    {
        public Guid TripID { get; set; }
        public Guid RouteID { get; set; }
        public string? StartLocation { get; set; } 
        public string? EndLocation { get; set; } 
        public string? StartDate { get; set; } 
        public string? StartTime { get; set; } 
        public int TotalSeats { get; set; }
        public List<string> SeatBooked { get; set; } = new List<string>();
        public List<TicketType_TripResponse> TicketType_TripResponses { get; set; }


        public class TicketType_TripResponse
        {
            public Guid TicketType_TripID { get; set; }
            public string? TicketName { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
        }
    }
}
