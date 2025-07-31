using System.Security.Claims;
using FUEM.Application.Interfaces.NotificationUseCases;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Web.Hubs;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FUEM.Web.Controllers
{
    public class AdminNotificationController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IAdminNotification _adminNotificationUseCase;

        public AdminNotificationController(IHubContext<NotificationHub> hubContext, IAdminNotification adminNotificationUseCase)
        {
            _hubContext = hubContext;
            _adminNotificationUseCase = adminNotificationUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Send(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim == null || !int.TryParse(userIdClaim.Value, out int adminId))
                {
                return RedirectToAction("Login", "Authentication");
            }

            ViewBag.CurrentUserId = userIdClaim.Value;

            var eventsPage = await _adminNotificationUseCase.GetEventsByOrganizerIdAsync(adminId, page, pageSize);
            ViewBag.Events = eventsPage.Items;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification(string message, string title, string targetType, int? eventId)
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
            switch (targetType)
            {
                case "AllStudents":
                    receiverIds = await _adminNotificationUseCase.GetAllStudentIdsAsync();
                    break;
                case "SpecificEvent":
                    if (!eventId.HasValue || eventId.Value <= 0)
                    {
                        return BadRequest("Please choose a specific event.");
                    }
                    receiverIds = await _adminNotificationUseCase.GetStudentIdsByEventIdAsync(eventId.Value);
                    break;
                case "ClubPresidents":
                    receiverIds = await _adminNotificationUseCase.GetClubPresidentIdsAsync();
                    isOrganizerReceivers = true;
                    break;

                default:
                    return BadRequest("Object type is invalid.");
            }

            var notification = await _adminNotificationUseCase.SendAndSaveNotificationAsync(senderId, title, message, receiverIds, isOrganizerReceivers);

            if(notification == null)
            {
                return StatusCode(500, "Failed to send notification.");
            }

            foreach (var id in receiverIds)
            {
                await _hubContext.Clients.User(id.ToString()).SendAsync("ReceiveNotification", notification.Title, message, notification.Id);
            }
            return Ok(new { message = $"Event has been sent successfully to {receiverIds.Count} receipents." });
        }
    }
}
