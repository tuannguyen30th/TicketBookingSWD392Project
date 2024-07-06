using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class FeedbackInTripResponse
    {
        [JsonPropertyName("Feedbacks")]
        public List<FeedbackModel> Feedbacks { get; set; }

        [JsonPropertyName("TotalRating")]
        public double TotalRating { get; set; }
        [JsonPropertyName("TotalPage")]

        public int TotalPages { get; set; }


    }
}
