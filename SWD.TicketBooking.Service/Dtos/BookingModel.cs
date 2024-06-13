/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class BookingModel
    {
        public class AddOrUpdateBookingModel
        {
            public Guid? BookingID { get; set; }
            public Guid UserID { get; set; }
            public Guid TripID { get; set; }
            public int Quantity { get; set; }
            public double TotalBill { get; set; }
            public string Status { get; set; } = string.Empty;
            public List<AddOrUpdateTicketModel> TicketModels { get; set; }
        }

        public class AddOrUpdateTicketModel
        {
            public Guid? TicketDetailID { get; set; }
            public Guid BookingID { get; set; }
            public Guid TicketType_TripID { get; set; }
            public double Price { get; set; }
            public string SeatCode { get; set; } = string.Empty;
            public List<AddOrUpdateServiceModel> ServiceModels { get; set; }
        }

        public class AddOrUpdateServiceModel
        {
            public Guid? TicketDetail_ServiceID { get; set; }
            public Guid? TicketDetailID { get; set; }
            public Guid ServiceID { get; set; }
            public Guid StationID { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
        }

    }
}
*/