using System.Security.Claims;
using FUEM.Application.Interfaces.NotificationUseCases;
using Microsoft.AspNetCore.Mvc;

namespace FUEM.Web.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentNotificationController : Controller
    {
        private readonly IStudentNotification _studentNotification;
        public StudentNotificationController(IStudentNotification studentNotification)
        {
            _studentNotification = studentNotification;
        }
        [HttpGet("/Student/Notifications")]
        public IActionResult Index()
        {
            // Không cần truyền dữ liệu ban đầu từ đây, sẽ load bằng AJAX ở frontend
            return View("~/Views/Student/Notifications/Index.cshtml"); // Trả về đường dẫn cụ thể của View
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetStudentNotifications()
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (studentIdClaim == null || !int.TryParse(studentIdClaim.Value, out int studentId))
            {
                return Unauthorized("You must be logged in to view notifications.");
            }
            var notifications = await _studentNotification.GetStudentNotificationsAsync(studentId);
            foreach(var noti in notifications)
            {
                if(noti.Title == null)
                {
                    noti.Title = "Notification without title";
                }
            }
            return Ok(notifications);
        }

        [HttpGet("notifications/count")]
        public async Task<IActionResult> GetStudentNotificationCount()
        {
            var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (studentIdClaim == null || !int.TryParse(studentIdClaim.Value, out int studentId))
            {
                return Unauthorized("You must be logged in to view notification count.");
            }
            var count = await _studentNotification.GetStudentNotificationCountAsync(studentId);
            return Ok(count);
        }
    }
}
