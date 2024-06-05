using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Station_Route")]
    public class Station_Route
    {
        [Key]
        public int Station_RouteID { get; set; }
        public int StationID { get; set; }
        [ForeignKey("StationID")]
        public Station Station { get; set; }
        public int RouteID { get; set; }
        [ForeignKey("RouteID")]
        public Route Route { get; set; }
        public string Status { get; set; }
        public int OrderInRoute {  get; set; }
    }

    
}
