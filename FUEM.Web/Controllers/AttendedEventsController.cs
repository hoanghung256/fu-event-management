using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using FUEM.Domain.Common;
using System.Collections.Generic;

namespace FUEM.Web.Controllers
{
    public class AttendedEventsController : Controller
    {
        private readonly ILogger<AttendedEventsController> _logger;
        private readonly IGetAttendedEvents _getAttendedEventsUseCase;
        private readonly IFeedback _feedbackService;
        private readonly FirebaseStorageService _storage;

        public AttendedEventsController(ILogger<AttendedEventsController> logger, IFeedback feedbackService, IGetAttendedEvents getAttendedEventsUseCase, FirebaseStorageService storage)
        {
            _logger = logger;
            _getAttendedEventsUseCase = getAttendedEventsUseCase;
            _storage = storage;
            _feedbackService = feedbackService;
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index([FromQuery] int pageNumber = 1)
        {
            // Lấy userId dưới dạng string cho GetAttendedEventsForUserAsync
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                _logger.LogWarning("UserId string not found in claims for authenticated user.");
                return Unauthorized();
            }

            // Lấy userId dưới dạng int cho các hoạt động liên quan đến GuestId (ví dụ: feedback)
            if (!int.TryParse(userIdString, out int userIdInt))
            {
                _logger.LogWarning("UserId could not be parsed to int for feedback check.");
                return Unauthorized();
            }

            Page<Event> eventsPage = await _getAttendedEventsUseCase.GetAttendedEventsForUserAsync(userIdString, pageNumber, 10);

       
            foreach (var eventItem in eventsPage.Items)
            {
                eventItem.HasSubmittedFeedback = await _feedbackService.CheckUserFeedbackForEventAsync(userIdInt, eventItem.Id);
            }

            return View("~/Views/Home/AttendedEvents.cshtml", eventsPage);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitFeedback(int eventId, string comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Invalid authentication" });
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int guestId))
            {
                return Json(new { success = false, message = "System can not get the userID. Please sign in again!" });
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                return Json(new { success = false, message = "Feedback content can not be null." });
            }
            if (comment.Length > 500)
            {
                return Json(new { success = false, message = "Feedback content was over 500 characters." });
            }

            var feedback = new Feedback
            {
                EventId = eventId,
                GuestId = guestId,
                Content = comment,
            };

            try
            {
                await _feedbackService.AddFeedbackAsync(feedback);
                return Json(new { success = true, message = "Feedback đã được gửi thành công!" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Feedback submission failed: {ErrorMessage}", ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " An unexpected errors has been occured with {EventId} of {GuestId}.", eventId, guestId);
                return Json(new { success = false, message = "The error has been occured when sending your feedback" });
            }
        }
    }
}