using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class FeedbackInTripResponse
    {
        [JsonPropertyName("Feedbacks")]
        public List<Feedback> Feedbacks { get; set; }

        [JsonPropertyName("TotalRating")]
        public double TotalRating { get; set; }

    }
}
