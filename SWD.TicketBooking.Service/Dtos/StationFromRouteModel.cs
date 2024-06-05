using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class StationFromRouteModel
    {
        public int StationID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
