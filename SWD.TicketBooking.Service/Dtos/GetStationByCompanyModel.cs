using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetStationByCompanyModel
    {
        public Guid StationID { get; set; }
        public Guid CityID { get; set; }
        public string StationName { get; set; }
        public List<ServiceTypeInStationModel> ServiceTypeInStation { get; set; }
    }
}
