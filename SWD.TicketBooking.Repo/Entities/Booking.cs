using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public int TripID { get; set; }
        [ForeignKey("TripID")]
        public Trip Trip { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.Now;
        public int Quantity { get; set; }
        public string QRCodeImage { get; set; } = string.Empty;
        public string QRCodeText { get; set; } = string.Empty;
        public string QRCode {  get; set; } = string.Empty;
        public double TotalBill { get; set; }
        public string? PaymentMethod { get; set; } = string.Empty;
        public string? PaymentStatus { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
    }
}
