using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Route")]

    public class Route
    {
        [Key]
        public Guid RouteID { get; set; }
        public Guid? FromCityID { get; set; }
        [ForeignKey("FromCityID")]
        public City? FromCity { get; set; }
        public Guid? ToCityID { get; set; }
        [ForeignKey("ToCityID")]
        public City? ToCity { get; set; }
        public string? StartLocation { get; set; } = string.Empty;
        public string? EndLocation { get; set; } = string.Empty;
        public string? Status {  get; set; } = string.Empty;

    }
}
