using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Station_Company")]
    public class Station_Company
    {
        [Key]
        public Guid Station_CompanyID { get; set; }
        public Guid? StationID { get; set; }
        [ForeignKey("StationID")]
        public Station? Station { get; set; }
        public Guid? CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company? Company { get; set; }
        public string? Status { get; set; } = string.Empty;
    }
}
