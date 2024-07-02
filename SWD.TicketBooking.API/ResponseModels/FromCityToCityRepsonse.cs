using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class FromCityToCityRepsonse
    {
        public class CityInfo
        {
            [JsonPropertyName("CityID")]
            public Guid CityID { get; set; }

            [JsonPropertyName("CityName")]
            public string? CityName { get; set; }
        }

        public class CityResponse
        {
            [JsonPropertyName("FromCities")]
            public List<CityInfo> FromCities { get; set; }

            [JsonPropertyName("ToCities")]
            public List<CityInfo> ToCities { get; set; }
        }
       
    }
}
