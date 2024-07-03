using SWD.TicketBooking.Service.Dtos;
using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class GetStationByCompanyResponse
    {
        [JsonPropertyName("StationID")]
        public Guid StationID { get; set; }
        [JsonPropertyName("CityID")]
        public Guid CityID { get; set; }
        [JsonPropertyName("StationName")]
        public string StationName { get; set; }
        [JsonPropertyName("ServiceTypeInStation")]
        public List<ServiceTypeInStationModel> ServiceTypeInStation { get; set; }
    }
}
