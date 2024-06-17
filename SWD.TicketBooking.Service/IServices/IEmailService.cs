using SWD.TicketBooking.Repo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(MailData mailData);
    }
}
