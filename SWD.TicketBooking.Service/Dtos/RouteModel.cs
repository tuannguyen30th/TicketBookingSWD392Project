using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class RouteModel
    {
        public int RouteID { get; set; }
        public int FromCityID { get; set; }
        public int ToCityID { get; set; }
        public int CompanyID { get; set; }
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
