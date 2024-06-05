using Microsoft.AspNetCore.Http;
using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class FeedbackRequestModel
    {
            public int UserID { get; set; }
            public int TripID { get; set; }
            public int Rating { get; set; }
            public string Description { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public List<IFormFile> Files { get; set; }
     
    }
}
