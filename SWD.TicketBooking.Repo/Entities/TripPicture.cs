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
        public Guid TripPictureID { get; set; }
        public Guid? TripID { get; set; }
        [ForeignKey("TripID")]
        public Trip? Trip { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
    }
}
