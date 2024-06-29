namespace SWD.TicketBooking.Service.Dtos.User
{
    public class UpdateUserResponseModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
