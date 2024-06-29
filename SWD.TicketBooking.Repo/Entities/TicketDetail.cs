using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("TicketDetail")]
    public class TicketDetail
    {
        [Key]
        public Guid TicketDetailID { get; set; }
        public Guid? BookingID { get; set; }
        [ForeignKey("BookingID")]
        public Booking? Booking { get; set; }
        public Guid? TicketType_TripID { get; set; }
        [ForeignKey("TicketType_TripID")]
        public TicketType_Trip? TicketType_Trip { get; set; }
        public string? QRCodeImage { get; set; } = string.Empty;
        public string? QRCode { get; set; } = string.Empty;
        public double? Price { get; set; }
        public string? SeatCode { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
    }
}
