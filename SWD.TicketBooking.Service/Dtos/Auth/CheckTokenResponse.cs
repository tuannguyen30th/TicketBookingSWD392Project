using SWD.TicketBooking.Service.Dtos.User;
namespace SWD.TicketBooking.Service.Dtos.Auth
{
    public class CheckTokenResponse
    {
        public UserModel User { get; set; } = new UserModel();
        public Guid CompanyID { get; set; }

    }
}
