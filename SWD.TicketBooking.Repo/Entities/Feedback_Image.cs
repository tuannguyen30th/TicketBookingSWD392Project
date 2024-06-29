using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Feedback_Image")]
    public class Feedback_Image
    {
        [Key]
        public Guid Feedback_Image_ID { get; set; }
        public Guid? FeedbackID { get; set; }
        [ForeignKey("FeedbackID")]
        public Feedback? Feedback { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

    }
}
