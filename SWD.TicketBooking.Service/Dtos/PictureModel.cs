using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class PictureModel
    {
        public Guid TripId { get; set; }

        public string? ImageUrl { get; set; }

    }
}
