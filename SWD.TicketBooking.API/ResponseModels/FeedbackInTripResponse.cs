﻿using SWD.TicketBooking.Service.Dtos;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class FeedbackInTripResponse
    {
        public List<FeedbackModel> Feedbacks { get; set; }
        public double TotalRating { get; set; }

    }
}
