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
            public Guid UserID { get; set; }
            public Guid TripID { get; set; }
            public int Rating { get; set; }
            public string? Description { get; set; }
            public string? Status { get; set; } 
            public List<IFormFile> Files { get; set; }
     
    }
}
