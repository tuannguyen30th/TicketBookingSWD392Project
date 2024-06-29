using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("TicketType_Trip")]
    public class TicketType_Trip
    {
        [Key]
        public Guid TicketType_TripID { get; set; }
        public Guid? TicketTypeID { get; set; }
        [ForeignKey("TicketTypeID")]
        public TicketType? TicketType { get; set; }
        public Guid? TripID { get; set; }
        [ForeignKey("TripID")]
        public Trip? Trip { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Status { get; set; } = string.Empty;
    }
}
