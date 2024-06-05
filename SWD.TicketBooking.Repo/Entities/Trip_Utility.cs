using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Trip_Utility")]

    public class Trip_Utility
    {
        [Key]
        public int Trip_UtilityID { get; set; }
        public int TripID {  get; set; }
        [ForeignKey("TripID")]
        public Trip Trip { get; set; }
        public int UtilityID { get; set; }

        [ForeignKey("UtilityID")]
        public Utility Utility { get; set; }


        public string Status { get; set; } = string.Empty;
    }
}
