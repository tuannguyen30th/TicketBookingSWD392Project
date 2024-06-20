using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class RouteResponse
    {
        public Guid RouteID { get; set; }
        public Guid FromCityID { get; set; }
        public Guid ToCityID { get; set; }
        public Guid CompanyID { get; set; }
        public string? StartLocation { get; set; }
        public string? EndLocation { get; set; }
        public string? Status { get; set; }
    }
}
