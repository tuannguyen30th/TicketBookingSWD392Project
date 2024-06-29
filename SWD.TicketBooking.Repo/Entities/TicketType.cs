using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("TicketType")]
    public class TicketType
    {
        [Key]
        public Guid TicketTypeID { get; set; }
/*        public int RouteID { get; set; }
        [ForeignKey("RouteID")]
        public Route Route { get; set; }*/
        public string? Name {  get; set; } = string.Empty;

        public string? Status { get; set; } = string.Empty;
    }
}
