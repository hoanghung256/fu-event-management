using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using FUEM.Application.Interfaces;

namespace FUEM.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IProcessEvent _processEventUseCase;
        public AdminController(IEventRepository eventRepository, IProcessEvent processEventUseCase)
        {
            _eventRepository = eventRepository;
            _processEventUseCase = processEventUseCase;
        }

        public async Task<IActionResult> PendingEvents(int pageNumber = 1, int pageSize = 5)
        {
            var eventPage = await _eventRepository.GetPendingEventForAdmin(pageNumber, pageSize);

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
    }
}
