using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Booking
{
    public class AddOrUpdateServiceModel
    {
        public Guid ServiceID { get; set; }
        public Guid StationID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
