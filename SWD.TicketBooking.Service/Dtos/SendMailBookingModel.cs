using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class SendMailBookingModel
    {
        public class MailBookingModel
        {
            public string Email { get; set; }
            public double Price { get; set; }
            public string? FullName { get; set; }
            public string? FromTo { get; set; }
            public string? StartTime { get; set; }
            public string? StartDate { get; set; }
            public string? SeatCode { get; set; }
            public string? TotalBill { get; set; }
            public string? QrCodeImage { get; set; }
            public List<MailBookingServiceModel> MailBookingServices { get; set; }
        }
        public class MailBookingServiceModel
        {
            public double ServicePrice { get; set; }
            public string AtStation { get; set; }
        }
    }
}
