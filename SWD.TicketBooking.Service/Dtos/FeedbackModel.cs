using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class FeedbackModel
    {
        public string? UserName {  get; set; } 
        public DateTime Date { get; set; }
        public string? Desciption { get; set; }
        public List<string> ImageUrl { get; set; } = new List<string>();
        public int Rating { get; set; }
        public string? Avt {  get; set; }
    }
}
