using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Feedback")]
    public class Feedback
    {
        [Key]
        public Guid FeedbackID { get; set; }
        public Guid? UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
        public Guid? TripID { get; set; }
        [ForeignKey("TripID")]
        public Trip? Trip { get; set; }
        public Guid? TemplateID { get; set; }
        [ForeignKey("TemplateID")]
        public Trip? TripTemplate { get; set; }
        public int? Rating { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

    }
}
