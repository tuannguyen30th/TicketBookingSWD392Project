using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateStationModel
    {
        public Guid CityId {  get; set; }
        public Guid CompanyId {  get; set; }
        public string? StationName { get; set; }
        public Guid RouteId { get; set; }
        public int OrderInRoute { get; set; }

    }
}
