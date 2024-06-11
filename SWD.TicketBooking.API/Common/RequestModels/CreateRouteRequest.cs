using SWD.TicketBooking.Repo.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.API.Common.RequestModels
{
    public class CreateRouteRequest
    {
        public Guid FromCityID { get; set; }
        public Guid ToCityID { get; set; }
        public Guid CompanyID { get; set; }
        public string? StartLocation { get; set; } 
        public string? EndLocation { get; set; } 
    }
}
