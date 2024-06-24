using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Booking
{
    public class AddOrUpdateBookingModel
    {
        public Guid UserID { get; set; }
        public Guid TripID { get; set; }
        public bool IsBalance {  get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double TotalBill { get; set; }
    }
}
