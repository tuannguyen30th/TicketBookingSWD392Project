namespace SWD.TicketBooking.Service.Dtos
{
    public class GetServiceTypeModel
    {
        public Guid ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; } = string.Empty;
        public List<GetServiceModel> Services { get; set; }
    }

    public class GetServiceModel
    {
        public Guid ServiceID { get; set; }
        public string ServiceName { get; set; } = string.Empty;
    }
}
