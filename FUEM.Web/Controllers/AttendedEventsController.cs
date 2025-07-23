using FUEM.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUEM.Web.Controllers
{
    public class AttendedEventsController : Controller
    {
        private readonly ILogger<AttendedEventsController> _logger;
        private readonly IGetAttendedEvents _getAttendedEventsUseCase;
        private readonly FirebaseStorageService _storage;
        public AttendedEventsController(ILogger<AttendedEventsController> logger, IGetAttendedEvents getAttendedEventsUseCase, FirebaseStorageService storage)
        {
            _logger = logger;
            _getAttendedEventsUseCase = getAttendedEventsUseCase;
            _storage = storage;
        }
        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index([FromQuery] int pageNumber = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var eventsPage = await _getAttendedEventsUseCase.GetAttendedEventsForUserAsync(userId, pageNumber, 10);

            return View("~/Views/Home/AttendedEvents.cshtml", eventsPage);
        }
    }

}