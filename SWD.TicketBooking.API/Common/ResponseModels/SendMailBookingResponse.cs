namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class SendMailBookingResponse
    {
        public class MailBookingResponse
        {
            public double Price { get; set; }
            public string? FullName { get; set; }
            public string? FromTo { get; set; }
            public string? StartTime { get; set; }
            public string? StartDate { get; set; }
            public string? SeatCode { get; set; }
            public string? TotalBill { get; set; }
            public string? QrCodeImage { get; set; }
            public List<MailBookingServiceResponse> MailBookingServices { get; set; }
        }
        public class MailBookingServiceResponse
        {
            public double ServicePrice { get; set; }
            public string AtStation { get; set; }
        }
    }
}
