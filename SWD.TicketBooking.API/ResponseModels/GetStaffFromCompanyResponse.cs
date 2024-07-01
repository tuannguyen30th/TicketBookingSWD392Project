namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetStaffFromCompanyResponse
    {
        public Guid StaffID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
