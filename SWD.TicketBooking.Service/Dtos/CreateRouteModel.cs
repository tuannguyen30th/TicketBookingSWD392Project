using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateRouteModel
    {
        public Guid FromCityID { get; set; }
        public Guid? ToCityID { get; set; } = Guid.Empty;
        public Guid? CompanyID { get; set; } = Guid.Empty;
        public string? StartLocation { get; set; } = string.Empty;
        public string? EndLocation { get; set; } =string.Empty;
        public string? StationCompany { get; set; } = string.Empty;

        public List<StationInRouteModel>? StationInRoutes { get; set; }
    }

    public class StationInRouteModel
    {
        public Guid StationID { get; set; } = Guid.Empty;
        public int? OrderInRoute {  get; set; }
    }
}
