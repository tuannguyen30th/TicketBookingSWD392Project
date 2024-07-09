using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("TicketDetail_Service")]
    public class TicketDetail_Service
    {
        [Key]
        public Guid TicketDetail_ServiceID { get; set; }
        public Guid? TicketDetailID { get; set; }
        [ForeignKey("TicketDetailID")]
        public TicketDetail? TicketDetail { get; set; }
        public Guid? ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public Service? Service { get; set; }
        public Guid? StationID { get; set; }
        [ForeignKey("StationID")]
        public Station? Station { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public bool? HasCheck { get; set; }
        public string? Status { get; set; } = string.Empty;

    }
}
