using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateStationModel
    {
        public int cityId {  get; set; }
        public int companyId {  get; set; }
        public string stationName { get; set; }
    }
}
