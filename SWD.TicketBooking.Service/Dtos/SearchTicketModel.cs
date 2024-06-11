using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class SearchTicketModel
    {
        public PriceInSearchTicketModel Price { get; set; }
        public TripInSearchTicketModel Trip { get; set; }
        public double TotalBill { get; set; }
        public string? QrCodeImage { get; set; }
    }
}
