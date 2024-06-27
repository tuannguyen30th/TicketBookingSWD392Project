using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace SWD.TicketBooking.API.Common
{
    public class User : IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        // Additional properties specific to your application

        // Implement IUser<TKey>
        public virtual int GetUserId() => Id;
        public virtual string GetUserName() => UserName;
        public virtual void SetUserName(string userName) => UserName = userName;
        // Implement other IUser methods as needed
    }
}
