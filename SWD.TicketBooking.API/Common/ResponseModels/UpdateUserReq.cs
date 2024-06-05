namespace SWD.TicketBooking.API.Common.ResponseModels
{
    public class UpdateUserReq
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public String confirmPassword { get; set; }


        public string FullName { get; set; }

        public string Avatar { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
