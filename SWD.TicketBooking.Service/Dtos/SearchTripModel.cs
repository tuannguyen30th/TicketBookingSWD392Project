﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class SearchTripModel
    {
        public int TripID { get; set; }
        public int RouteID { get; set; }
        public string CompanyName { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public int QuantityRating { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int EmptySeat { get; set; }
        public double Price { get; set; }
    }
}
