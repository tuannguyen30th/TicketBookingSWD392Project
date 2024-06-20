namespace SWD.TicketBooking.API.RequestModels.Booking
{
    public class AddOrUpdateBookingRequest
    {
        public Guid UserID { get; set; }
        public Guid TripID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double TotalBill { get; set; }
    }
}
