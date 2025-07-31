using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FUEM.Web.Controllers
{
    public class OrganizedEventController : Controller
    {
        private readonly ILogger<OrganizedEventController> _logger;
        private readonly IGetOrganizedEvents _getOrganizedEventsUseCase;
        private readonly FirebaseStorageService _storage;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IEventRepository _eventRepository; // Đảm bảo IEventRepository có phương thức GetEventByIdAsync(int id)

        public OrganizedEventController(
            ILogger<OrganizedEventController> logger,
            IGetOrganizedEvents getOrganizedEventsUseCase,
            FirebaseStorageService storage,
            IFeedbackRepository feedbackRepository,
            IEventRepository eventRepository)
        {
            _logger = logger;
            _getOrganizedEventsUseCase = getOrganizedEventsUseCase;
            _storage = storage;
            _feedbackRepository = feedbackRepository;
            _eventRepository = eventRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Club")]
        public async Task<IActionResult> Index([FromQuery] int pageNumber = 1)
        {
            Page<Event> eventsPage;
            var organizedId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.IsInRole("Admin"))
            {
                eventsPage = await _getOrganizedEventsUseCase.GetOrganizedEventsForAdminAsync(pageNumber, 10);
            }
            else
            {
                eventsPage = await _getOrganizedEventsUseCase.GetOrganizedEventsForOrganizerAsync(organizedId, pageNumber, 10);
            }

            return View($"~/Views/{User.FindFirstValue(ClaimTypes.Role)}/OrganizedEvent.cshtml", eventsPage);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Club")]
        public async Task<IActionResult> ViewFeedback(int eventId)
        {
            var ev = await _eventRepository.GetEventByIdAsync(eventId);
            if (ev == null)
            {
                _logger.LogWarning($"Event with ID {eventId} not found for feedback view.");
                return NotFound();
            }

            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (!User.IsInRole("Admin"))
            {
                int currentUserIdInt;
                if (!int.TryParse(currentUserIdString, out currentUserIdInt))
                {
                    _logger.LogError($"Could not parse currentUserId '{currentUserIdString}' to int.");
                    return Unauthorized(); 
                }

                if (!ev.OrganizerId.HasValue || ev.OrganizerId.Value != currentUserIdInt)
                {
                    _logger.LogWarning($"User {currentUserIdString} attempted to view feedback for event {eventId} without authorization (not owner). OrganizerId: {ev.OrganizerId}.");
                    return Unauthorized();
                }
            }

            var feedbacks = await _feedbackRepository.GetFeedbacksByEventIdAsync(eventId);

          
            ViewBag.EventName = ev.Fullname; 
            ViewBag.EventId = ev.Id;
            return View("~/Views/Club/FeedbackOfEvent.cshtml", feedbacks);
        }
    }
}