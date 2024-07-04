using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class UpdateStationModel
    {
        public Guid CityId { get; set; }
        public Guid CompanyId { get; set; }
        public string? StationName { get; set; }
    }
}
