using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface INotificationService
    {
        Task<string> SendNotification(string token, string title, string body);

    }
}