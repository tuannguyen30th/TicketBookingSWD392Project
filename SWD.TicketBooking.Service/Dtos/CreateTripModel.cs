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
    public class CreateTripModel
    {
        public Guid Route_CompanyID { get; set; }
        public bool IsTemplate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Status { get; set; } 
        public List<IFormFile> ImageUrls { get; set; } = new List<IFormFile>();
        public List<TicketType_TripModel> TicketType_TripModels { get; set; } = new List<TicketType_TripModel>();
        public List<Trip_UtilityModel> Trip_UtilityModels { get; set; } = new List<Trip_UtilityModel>();

        public class TicketType_TripModel
        {
            public Guid TicketTypeID { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
        }
        public class Trip_UtilityModel
        {         
            public Guid UtilityID { get; set; }
        }
    }
}