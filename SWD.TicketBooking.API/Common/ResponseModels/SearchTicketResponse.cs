using SWD.TicketBooking.Service.Dtos;

namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class SearchTicketResponse
    {
        public PriceInSearchTicketModel price { get; set; }
        public TripInSearchTicketModel trip { get; set; }
        public double TotalBill { get; set; }
        public string? QrCodeImage { get; set; }
    }
}
