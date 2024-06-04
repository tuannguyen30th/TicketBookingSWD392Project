using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Trip")]
    public class Trip
    {
        [Key]
        public int TripID { get; set; }
        public int RouteID { get; set; }
        [ForeignKey("RouteID")]
        public Route Route { get; set; }
        public bool IsTemplate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;


    }
}
