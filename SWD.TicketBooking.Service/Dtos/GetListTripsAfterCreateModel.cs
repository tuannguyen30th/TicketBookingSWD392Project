using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetListTripsAfterCreateModel
    {
        public Guid? TripID { get; set; }
        public Guid? Route_CompanyID { get; set; }
        public bool? IsTemplate { get; set; }
        public Guid? StaffID { get; set; }
        public Guid? TemplateID { get; set; }
        public GetTimeTripAfterCreateModel GetTimeTripAfterCreateModels { get; set; } = new GetTimeTripAfterCreateModel();
        public class GetTimeTripAfterCreateModel
        {
            public DateTime? StartTime { get; set; }
            public DateTime? EndTime { get; set; }
        }
      
    }
}
