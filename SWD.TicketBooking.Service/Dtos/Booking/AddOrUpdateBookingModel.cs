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
        public int Quantity { get; set; }
        public double TotalBill { get; set; }
    }
}
