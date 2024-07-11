using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class UpdateServiceInStationModel
    {
        public double Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
