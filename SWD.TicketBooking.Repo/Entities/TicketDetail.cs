using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("TicketDetail")]
    public class TicketDetail
    {
        [Key]
        public int TicketDetailID { get; set; }
        public int BookingID { get; set; }
        [ForeignKey("BookingID")]
        public Booking Booking { get; set; }
        public int TicketType_TripID { get; set; }
        [ForeignKey("TicketType_TripID")]
        public TicketType_Trip TicketType_Trip { get; set; }
        public double Price { get; set; }
        public string SeatCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
