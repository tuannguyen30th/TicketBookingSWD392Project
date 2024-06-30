namespace SWD.TicketBooking.API.ResponseModels
{

    public class GetTicketDetailInMobileResponse
    {
        public string QrCodeImage { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string StartTime { get; set; }
        public string StartDay { get; set; }
        public string SeatCode { get; set; }
        public string Route { get; set; }
        public List<ServiceInTicketResponse> Services { get; set; }
    }

    public class ServiceInTicketResponse
    {
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public string Station { get; set; }
        public double TotalPrice { get; set; }

    }
}
