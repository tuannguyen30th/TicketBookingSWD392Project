using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class CreateServiceModel
    {
        public Guid ServiceTypeID { get; set; }
        public string? Name { get; set; }
    }
}
