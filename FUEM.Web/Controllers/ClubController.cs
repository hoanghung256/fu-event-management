using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Application.Interfaces.StudentUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace FUEM.Web.Controllers
{
    public class ClubController : Controller
    {
        private readonly IGetOrganizer _getOrganizerUseCase;
        private readonly IGetRecentEvents _getRecentEventsUseCase;
        private readonly IFollowUseCase _followUseCase;
        private readonly IGetStudent _getStudentUseCase;
        private readonly IEditOrganizer _editOrganizerUseCase;
        private readonly ICompareEventUseCase _compareEventUseCase;

        public ClubController(IGetOrganizer getOrganizerUseCase, IGetRecentEvents getRecentEventsUseCase, IFollowUseCase followUseCase, IGetStudent getStudentUseCase, IEditOrganizer editOrganizerUseCase, ICompareEventUseCase compareEventUseCase)
        {
            _getOrganizerUseCase = getOrganizerUseCase;
            _getRecentEventsUseCase = getRecentEventsUseCase;
            _followUseCase = followUseCase;
            _getStudentUseCase = getStudentUseCase;
            _editOrganizerUseCase = editOrganizerUseCase;
            _compareEventUseCase = compareEventUseCase;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Student, Club, Admin")]
        public async Task<ActionResult> ProfileAsync(int? id)
        {
            List<Event> recentEvents = id == null ? await _getRecentEventsUseCase.GetRecentEventsByOrganizerId(int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value)) : await _getRecentEventsUseCase.GetRecentEventsByOrganizerId(id.Value);
            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value == Role.Club.ToString() || User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value == Role.Admin.ToString())
            {
                var org = await _getOrganizerUseCase.GetOrganizerByEmailAsync(User.FindFirst(System.Security.Claims.ClaimTypes.Email).Value);
                ViewBag.RecentEvents = recentEvents;
                return View(org);
            }
            else if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value == Role.Student.ToString())
            {
                var org = await _getOrganizerUseCase.GetOrganizerByIdAsync(id.Value);
                var stu = await _getStudentUseCase.GetStudentByEmail(User.FindFirst(System.Security.Claims.ClaimTypes.Email).Value);
                ViewBag.IsFollowing = await _followUseCase.isUserFollowing(org.Id, stu.Id);
                ViewBag.RecentEvents = recentEvents;
                return View(org);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Club")]
        public async Task<ActionResult> EditProfileAsync(Organizer? model, [FromForm] IFormFile? coverFile, [FromForm] IFormFile? avatarFile)
        {
            if (model == null)
            {
                return RedirectToAction("404", "Error");
            }
            Organizer organizer = await _getOrganizerUseCase.GetOrganizerByIdAsync(model.Id);
            if (model != null)
            {
                organizer.Fullname = model.Fullname;
                organizer.Acronym = model.Acronym;
                organizer.Description = model.Description;
                organizer.Email = model.Email;
            }
            try
            {
                Stream coverStream = null;
                if (coverFile != null)
                {
                    coverStream = coverFile.OpenReadStream();
                    organizer.CoverPath = coverFile.FileName;
                    Console.WriteLine(organizer.CoverPath);
                }
                Stream avatarStream = null;
                if (avatarFile != null)
                {
                    avatarStream = avatarFile.OpenReadStream();
                    organizer.AvatarPath = avatarFile.FileName;
                    Console.WriteLine(organizer.AvatarPath);
                }
                await _editOrganizerUseCase.EditOrganizerAsync(organizer, avatarStream, coverStream);
                if (coverFile != null)
                    await coverStream.DisposeAsync();
                if (avatarFile != null)
                    await avatarStream.DisposeAsync();
                TempData[ToastType.SuccessMessage.ToString()] = "Edit profile successfully";
            }
            catch
            {
                TempData[ToastType.ErrorMessage.ToString()] = "Edit profile failed";
            }
            List<Event> recentEvents = await _getRecentEventsUseCase.GetRecentEventsByOrganizerId(int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value));
            var org = await _getOrganizerUseCase.GetOrganizerByEmailAsync(User.FindFirst(System.Security.Claims.ClaimTypes.Email).Value);
            //org.AvatarPath = await _firebase.GetSignedFileUrlAsync(org.AvatarPath);
            //org.CoverPath = await _firebase.GetSignedFileUrlAsync(org.CoverPath);
            ViewBag.RecentEvents = recentEvents;
            return RedirectToAction("Profile", org);
        }

        [HttpGet("Club/CompareEvents")]
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
