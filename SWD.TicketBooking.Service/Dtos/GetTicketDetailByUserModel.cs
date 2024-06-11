using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetTicketDetailByUserModel
    {
        public Guid BookingID { get; set; }
        public Guid TicketDetailID { get; set; }
        public string? CompanyName { get; set; }
        public string? StartDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndDate { get; set; }
        public string? EndTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public string? StartCity { get; set; }
        public string? EndCity { get; set; }
        public string? SeatCode { get; set; }
        public double TicketPrice { get; set; }
        public double TotalServicePrice { get; set; }
        public string? Status { get; set; }
    }
}
