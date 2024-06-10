﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class GetDetailTicketDetailByTicketDetailModel
    {
        public int BookingID { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public string SeatCode { get; set; }
        public double TicketPrice { get; set; }
        public double TotalServicePrice { get; set; }
        public double SumOfPrice { get; set; }
        public string Status { get; set; }
        public string QrCodeImage { get; set; }
        public string QrCode { get; set; }
        public List<ServiceDetailModel> ServiceDetailList { get; set; }
    }

    public class ServiceDetailModel
    {
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public double ServicePrice { get; set; }
        public string ServiceInStation { get; set; }
    }
}
