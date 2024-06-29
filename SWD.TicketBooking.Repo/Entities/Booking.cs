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
        public Guid BookingID { get; set; }
        public Guid? UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
        public Guid? TripID { get; set; }
        [ForeignKey("TripID")]
        public Trip? Trip { get; set; }
        public string? FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public DateTime? BookingTime { get; set; } = DateTime.Now;
        public int? Quantity { get; set; }
        public double? TotalVnpayPayment { get; set; }
        public double? TotalBalancePayment { get; set; }
        public double? TotalBill { get; set; }
      /*  public double? TotalVnPayPayments { get; set; }
        public double? TotalRemainingBalance { get; set; }*/
        public string? PaymentStatus { get; set; } = string.Empty;
    }
}
