using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Station_Service")]
    public class Station_Service
    {
        [Key]
        public Guid Station_ServiceID { get; set; }
        public Guid? StationID { get; set; }
        [ForeignKey("StationID")]
        public Station? Station { get; set; }
        public Guid? ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public Service? Service { get; set; }
        public double? Price { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
    }
}
