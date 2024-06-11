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
    public class CreateServiceInStationModel
    {
        public Guid StationID { get; set; }
        public Guid ServiceID { get; set; }
        public double Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
