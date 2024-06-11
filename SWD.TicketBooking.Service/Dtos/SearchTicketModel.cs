using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class SearchTicketModel
    {
        public PriceInSearchTicketModel price { get; set; }
        public TripInSearchTicketModel trip { get; set; }
        public double TotalBill { get; set; }
        public string QrCodeImage { get; set; }
    }
}
