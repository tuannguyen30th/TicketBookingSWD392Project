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
        public Guid TripID { get; set; }
        public Guid RouteID { get; set; }
        public string? CompanyName { get; set; }
        public string? StartLocation { get; set; } 
        public string? EndLocation { get; set; } 
        public string? StartDate { get; set; } 
        public string? StartTime { get; set; } 
        public int TotalSeats {  get; set; }
        public List<string> SeatBooked { get; set; } = new List<string>();
        public List<TicketType_TripModel> TicketType_TripModels { get; set; } 


        public class TicketType_TripModel
        {
            public Guid TicketType_TripID { get; set; }
            public string? TicketName { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
        }

    }
}
