using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Dtos
{
    public class SearchTicketModel
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
