using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("TicketDetail_Service")]
    public class TicketDetail_Service
    {
        [Key]
        public int TicketDetail_ServiceID { get; set; }
        public int TicketDetailID { get; set; }
        [ForeignKey("TicketDetailID")]
        public TicketDetail TicketDetail { get; set; }
        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public Service Service { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
