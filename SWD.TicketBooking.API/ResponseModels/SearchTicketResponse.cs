using SWD.TicketBooking.Service.Dtos;
using System.Text.Json.Serialization;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class SearchTicketResponse
    {
        [JsonPropertyName("Price")]
        public PriceInSearchTicketModel Price { get; set; }

        [JsonPropertyName("Trip")]
        public TripInSearchTicketModel Trip { get; set; }

        [JsonPropertyName("TotalBill")]
        public double TotalBill { get; set; }

        [JsonPropertyName("QrCodeImage")]
        public string? QrCodeImage { get; set; }

        [JsonPropertyName("QrCode")]
        public string? QrCode { get; set; }

    }
}
