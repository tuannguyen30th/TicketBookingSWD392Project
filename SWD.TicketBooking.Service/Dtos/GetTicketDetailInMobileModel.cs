using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetTicketDetailInMobileModel
    {
        public string QrCodeImage { get; set; }
        public string Status {  get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string StartTime { get; set; }
        public string StartDay { get; set; }
        public string SeatCode { get; set; }
        public string Route {  get; set; }
        public List<ServiceInTicketModel> Services { get; set; }
    }

    public class  ServiceInTicketModel
    {
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public string Station { get; set; }
        public double TotalPrice { get; set; }
    }
}
