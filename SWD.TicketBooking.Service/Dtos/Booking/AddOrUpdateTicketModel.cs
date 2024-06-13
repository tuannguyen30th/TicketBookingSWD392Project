using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos.Booking
{
    public class AddOrUpdateTicketModel
    {
        public Guid TicketType_TripID { get; set; }
        public double Price { get; set; }
        public string SeatCode { get; set; } = string.Empty;
        public List<AddOrUpdateServiceModel> AddOrUpdateServiceModels { get; set; }

    }
}
