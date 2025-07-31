using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using FUEM.Application.Interfaces.EventUseCases;

namespace FUEM.Web.Controllers
{
    public class OrganizedEventController : Controller
    {
        private readonly ILogger<OrganizedEventController> _logger;
        private readonly IGetOrganizedEvents _getOrganizedEventsUseCase;
        private readonly FirebaseStorageService _storage;

        public OrganizedEventController(ILogger<OrganizedEventController> logger, IGetOrganizedEvents getOrganizedEventsUseCase, FirebaseStorageService storage)
        {
            _logger = logger;
            _getOrganizedEventsUseCase = getOrganizedEventsUseCase;
            _storage = storage;
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
    }
}