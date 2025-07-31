using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using FUEM.Application.Interfaces;
using FUEM.Domain.Common;
using System.Security.Claims;

namespace FUEM.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IProcessEvent _processEventUseCase;
        private readonly IGetEventForGuest _getEvent;
        private readonly IGetOrganizedEvents _getOrganizedEvents;
        private readonly ICompareEventUseCase _compareEventUseCase;

        public AdminController(IEventRepository eventRepository, IProcessEvent processEventUseCase, ICompareEventUseCase compareEventUseCase, IGetOrganizedEvents getOrganizedEvents, IGetEventForGuest getEvent)
        {
            _eventRepository = eventRepository;
            _processEventUseCase = processEventUseCase;
            _compareEventUseCase = compareEventUseCase;
            _getOrganizedEvents = getOrganizedEvents;
            _getEvent = getEvent;
        }

        public async Task<IActionResult> PendingEvents(int pageNumber = 1, int pageSize = 5)
        {
            var eventPage = await _processEventUseCase.GetPendingEventForAdmin(pageNumber, pageSize);

            return View(eventPage);
        }


        [HttpGet("Admin/ApprovalEventDetails/{eventId}")]
        public async Task<IActionResult> ApprovalEventDetails(int eventId)
        {
            var eventDetails = await _eventRepository.GetEventByIdAsync(eventId);

            if (eventDetails == null)
            {
                return NotFound();
            }

            return View(eventDetails);
        }

        [HttpPost("Admin/ApproveEvent/{eventId}")]
        public async Task<IActionResult> ApproveEvent(int eventId, string action)
        {
            var eventToUpdate = await _eventRepository.GetEventByIdAsync(eventId);
            if (eventToUpdate == null)
            {
                return NotFound();
            }

            bool success = await _processEventUseCase.ProcessEventAsync(eventId, action);

            if (success)
            {
                TempData[ToastType.SuccessMessage.ToString()] = $"Event {eventToUpdate.Fullname} has been {action} successfully.";
            }
            else
            {
                TempData[ToastType.ErrorMessage.ToString()] = "An error occurred while processing your request.";
            }
            return RedirectToAction("PendingEvents");
        }

        [HttpGet("Admin/Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            Page<Event> pendingEventPage = await _processEventUseCase.GetPendingEventForAdmin(1, 10);
            Page<Event> upcommingEventPage = await _getEvent.GetUpcomingEventForAdminAsync(1, 10);
            Page<Event> organizedEventPage = await _getOrganizedEvents.GetOrganizedEventsForAdminAsync(1, 10);

            AdminDashboardViewModel adminDashboardViewModel = new()
            {
                PendingEventList = pendingEventPage.Items,
                UpcommingEventList = upcommingEventPage.Items,
                OrganizedEventThisMonthList = organizedEventPage.Items,
            };

            return View(adminDashboardViewModel);
        }
        
        [HttpGet("Admin/CompareEvents")]
        public async Task<IActionResult> CompareEvents(int eventId, int? comparedId)
        {
            string organizerIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recentOrganizedEvents = await _compareEventUseCase.GetRemainEventAsync(organizerIdStr, eventId, comparedId);
            ViewBag.RecentOrganizedEvents = recentOrganizedEvents;
            var eventList = await _compareEventUseCase.CompareEvent(eventId, comparedId);
            return View(eventList);
        }
    }
}