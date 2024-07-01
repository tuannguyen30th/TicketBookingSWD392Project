namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetRouteFromCompanyResponse
    {
        public Guid Route_CompanyID { get; set; }
        public string? FromCity { get; set; } = string.Empty;
        public string? ToCity { get; set; } = string.Empty;
        public string? StartLocation { get; set; } = string.Empty;
        public string? EndLocation { get; set; } = string.Empty;
       
    }
}
