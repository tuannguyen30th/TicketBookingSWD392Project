using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Route_Company")]
    public class Route_Company
    {
        [Key]
        public int Route_CompanyID { get; set; }
        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
        public int RouteID { get; set; }
        [ForeignKey("RouteID")]
        public Route Route { get; set; }
        public string Status { get; set; }
    }
}
