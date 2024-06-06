using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("TripPicture")]

    public class TripPicture
    {
        [Key]
        public int TripPictureID { get; set; }

        public string imageUrl { get; set; }
        public int TripID { get; set; }
        [ForeignKey("TripID")]
        public Trip Trip { get; set; }
        public string status { get; set; }
    }
}
