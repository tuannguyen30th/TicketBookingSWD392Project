﻿namespace SWD.TicketBooking.API.RequestModels
{
    public class UpdateUserRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public string FullName { get; set; }

        public IFormFile Avatar { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}