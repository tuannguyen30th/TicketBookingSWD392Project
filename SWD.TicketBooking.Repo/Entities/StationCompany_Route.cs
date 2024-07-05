using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("StationCompany_Route")]
    public class StationCompany_Route
    {
        [Key]
        public Guid StationCompany_RouteID { get; set; }
        public Guid? Station_CompanyID { get; set; }
        [ForeignKey("Station_CompanyID")]
        public Station_Company? Station_Company { get; set; }
        public Guid? RouteID { get; set; }
        [ForeignKey("RouteID")]
        public Route? Route { get; set; }
        public string? Status { get; set; } = string.Empty;
        public int? OrderInRoute { get; set; }
    }


}
