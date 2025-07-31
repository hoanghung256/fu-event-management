using System.Security.Claims;
using FUEM.Application.Interfaces.NotificationUseCases;
using FUEM.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FUEM.Web.Controllers
{
    [Authorize]
    public class ClubNotificationController : Controller
    {
        private readonly IClubNotification _clubNotification;
        private readonly IHubContext<NotificationHub> _hubContext;
        public ClubNotificationController(IClubNotification clubNotification, IHubContext<NotificationHub> hubContext)
        {
            _clubNotification = clubNotification;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("api/club/notifications")]
        public async Task<IActionResult> GetClubNotifications()
        {
            var clubIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (clubIdClaim == null || !int.TryParse(clubIdClaim.Value, out int clubId))
            {
                return Unauthorized("You must be logged in to view notifications.");
            }
            var notifications = await _clubNotification.GetClubNotificationsAsync(clubId);
            foreach (var noti in notifications)
            {
                if (noti.Title == null)
                {
                    noti.Title = "Notification without title";
                }
            }
            return Ok(notifications);
        }

        [HttpGet("api/club/notifications/count")]
        public async Task<IActionResult> GetClubNotificationCount()
        {
            var clubIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (clubIdClaim == null || !int.TryParse(clubIdClaim.Value, out int clubId))
            {
                return Unauthorized("You must be logged in to view notifications.");
            }
            var count = await _clubNotification.GetClubNotificationCountAsync(clubId);
            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> Send(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int clubId))
            {
                return RedirectToAction("Login", "Authentication");
            }

            ViewBag.CurrentUserId = userIdClaim.Value;

            var eventsPage = await _clubNotification.GetEventsByOrganizerIdAsync(clubId, page, pageSize);
            ViewBag.Events = eventsPage.Items;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification(string message, string title, int? eventId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("The message can not be empty.");
            }

            var senderIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (senderIdClaim == null || !int.TryParse(senderIdClaim.Value, out int senderId))
            {
                return Unauthorized("You must be logged in to send notifications.");
            }

            List<int> receiverIds = new List<int>();
            bool isOrganizerReceivers = false;
            if (!eventId.HasValue || eventId.Value <= 0)
            {
                return BadRequest("Please choose a specific event.");
            }
            receiverIds = await _clubNotification.GetStudentIdsByEventIdAsync(eventId.Value);

            var notification = await _clubNotification.SendAndSaveNotificationAsync(senderId, title, message, receiverIds, isOrganizerReceivers);

            if (notification == null)
            {
                return StatusCode(500, "Failed to send notification.");
            }

            foreach (var id in receiverIds)
            {
                await _hubContext.Clients.User(id.ToString()).SendAsync("ReceiveNotification", notification.Title, message, notification.Id, notification.Sender.Acronym);
            }
            return Ok(new { message = $"Event has been sent successfully to {receiverIds.Count} receipents." });
        }
    }
}
