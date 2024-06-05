using SWD.TicketBooking.Repo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetStationServiceModel
    {
        public int ServiceID { get; set; }
        public Route Route { get; set; }
        public ServiceType ServiceType { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
