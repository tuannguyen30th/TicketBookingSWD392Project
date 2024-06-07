using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class FeedbackModel
    {
        public string userName {  get; set; }
        public DateTime Date { get; set; }
        public string Desciption { get; set; }
        public List<string> imageUrl { get; set; }
        public int rating { get; set; }
        public string avt {  get; set; }
    }
}
