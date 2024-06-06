namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class UserResponse
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string Avatar { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public string OTPCode { get; set; }

    }
}
