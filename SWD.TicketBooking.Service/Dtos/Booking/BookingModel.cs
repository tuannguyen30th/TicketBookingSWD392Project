using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Booking
{
    public class BookingModel
    {
        public AddOrUpdateBookingModel AddOrUpdateBookingModel { get; set; }
        public List<AddOrUpdateTicketModel> AddOrUpdateTicketModels { get; set; }

    }
}
