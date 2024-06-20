namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetCompanyResponse
    {
        public Guid CompanyID { get; set; }
        public string Name { get; set; }
        public string? Status { get; set; }
    }
}
