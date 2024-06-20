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
    public class UpdateServiceModel
    {
        public Guid ServiceTypeID { get; set; }
        public string? Name { get; set; } 
    }
}
