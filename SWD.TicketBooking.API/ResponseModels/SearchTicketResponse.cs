using SWD.TicketBooking.Service.Dtos;

namespace SWD.TicketBooking.API.ResponseModels
{
    public class SearchTicketResponse
    {
        public PriceInSearchTicketModel Price { get; set; }
        public TripInSearchTicketModel Trip { get; set; }

        public double TotalBill { get; set; }
        public string? QrCodeImage { get; set; }
        public string? QrCode { get; set; }

    }
}
