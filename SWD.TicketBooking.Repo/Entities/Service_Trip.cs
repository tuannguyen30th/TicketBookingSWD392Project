/*using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Service_Trip")]
    public class Service_Trip
    {
        [Key]
        public int Service_TripID { get; set; }
        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public Service Service { get; set; }
        public int TripID { get; set; }
        [ForeignKey("TripID")]
        public Trip Trip { get; set; }
        public string Status { get; set; }
    }
}
*/