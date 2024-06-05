using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class RouteResponse
    {
        public int RouteID { get; set; }
        public int FromCityID { get; set; }
        public int ToCityID { get; set; }
        public int CompanyID { get; set; }
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
