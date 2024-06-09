using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetSeatBookedFromTripModel
    {
        public int TripID { get; set; }
        public int RouteID { get; set; }
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty ;
        public string StartDate { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public int TotalSeats {  get; set; }
        public List<string> SeatBooked { get; set; } = new List<string>();
        public List<TicketType_TripModel> TicketType_TripModels { get; set; }


        public class TicketType_TripModel
        {
            public int TicketType_TripID { get; set; }
            public string TicketName { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
        }

    }
}
