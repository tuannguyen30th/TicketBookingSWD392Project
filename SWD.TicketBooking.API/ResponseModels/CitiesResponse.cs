namespace SWD.TicketBooking.API.ResponseModels
{
    public class CitiesResponse
    {
        public Guid CityID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
