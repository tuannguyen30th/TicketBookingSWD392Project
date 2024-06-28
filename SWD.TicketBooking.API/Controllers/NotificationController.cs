using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.Service.IServices;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("notification-management")]
    [ApiController]
    public class NotificationController : Controller
    {
        public readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("managed-notifications")]
        public async Task<IActionResult> SendNotification(string token, string title, string body)
        {
            var result = await _notificationService.SendNotification(token, title, body);
            if (result != null)
            {
                return Ok(new { message = "Notification sent successfully" });
            }

            return BadRequest(new { message = "Failed to send notification" });
        }
    }
}
