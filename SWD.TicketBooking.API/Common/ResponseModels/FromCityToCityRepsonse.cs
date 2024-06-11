namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class FromCityToCityRepsonse
    {
        public class CityInfo
        {
            public Guid CityID { get; set; }
            public string? CityName { get; set; }
        }

        public class CityResponse
        {
            public List<CityInfo> FromCities { get; set; }
            public List<CityInfo> ToCities { get; set; }
        }
    }
}
