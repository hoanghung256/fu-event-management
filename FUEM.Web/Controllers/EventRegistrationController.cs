using FUEM.Application.Interfaces.CategoryUseCases;
using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Application.Interfaces.LocationUseCases;
using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Application.UseCases.EventUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FUEM.Web.Controllers
{
    public class EventRegistrationController : Controller
    {
        private readonly ILogger<EventRegistrationController> _logger;
        private readonly ICreateEvent _createEventUseCase;
        private readonly IRegisterIntoEvent _registerEventUseCase;
        private readonly IGetEventForGuest _getEventForGuestUseCase;
        private readonly IGetAllCategories _getAllCategoriesUseCase;
        private readonly IGetAllLocation _getAllLocationUseCase;

        public EventRegistrationController(ILogger<EventRegistrationController> logger, ICreateEvent createEventUseCase, IRegisterIntoEvent registerEventUseCase, IGetEventForGuest getEventForGuestUseCase, IGetAllCategories getAllCategoriesUseCase, IGetAllOrganizers getAllOrganizersUseCase, IGetAllLocation getAllLocationUseCase)
        {
            _logger = logger;
            _createEventUseCase = createEventUseCase;
            _registerEventUseCase = registerEventUseCase;
            _getEventForGuestUseCase = getEventForGuestUseCase;
            _getAllCategoriesUseCase = getAllCategoriesUseCase;
            _getAllLocationUseCase = getAllLocationUseCase;
        }

        [HttpGet]
        public async Task<ActionResult> Detail(int id)
        {
            var detailEvent = await _getEventForGuestUseCase.GetEventByIdAsync(id);
            var studentId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            TempData["isRegisteredAsGuest"] = await _registerEventUseCase.CheckIfResgisterAsGuest(id, studentId);
            TempData["isRegisteredAsCollaborator"] = await _registerEventUseCase.CheckIfResgisterAsCollaborator(id, studentId);

            return View(detailEvent);
        }

        [HttpPost]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> Register([FromForm]int eventId, [FromForm] bool isGuest)
        {
            try
            {
                var studentId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                if (isGuest)
                    await _registerEventUseCase.RegisterGuestAsync(eventId, studentId);
                else
                    await _registerEventUseCase.RegisterCollaboratorAsync(eventId, studentId);

                string role = isGuest ? "Guest" : "Collaborator";

                TempData[ToastType.SuccessMessage.ToString()] = $"Register as {role}";
                return RedirectToAction("Detail", new {id = eventId}); 
            }
            catch (Exception ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.Message;
                return RedirectToAction("Detail", new { id = eventId });
            }
        }


        // GET: EventRegistrationController
        [HttpGet]
        [Authorize(Roles = "Admin, Club")]
        public async Task<ActionResult> Create()
        {
            ViewData["Locations"] = await _getAllLocationUseCase.ExecuteAsync();
            ViewData["Categories"] = await _getAllCategoriesUseCase.GetAllCategoriesAsync();
            return View();
        }

        // POST: EventRegistrationController/Create
        [HttpPost]
        [Authorize(Roles = "Admin, Club")]
        public async Task<ActionResult> Create([FromForm] Event createEvent, [FromForm] IFormFile avatar, IFormFileCollection addtitionalImages)
        {
            try
            {
                Stream avatarStream = null;
                List<EventImage> additionalImages = new();

                if (avatar != null)
                {
                    avatarStream = avatar.OpenReadStream();
                    createEvent.AvatarPath = avatar.FileName;
                }

                if (addtitionalImages != null && addtitionalImages.Count > 0)
                {
                    foreach (var image in addtitionalImages)
                    {
                        additionalImages.Add(new EventImage()
                        {
                            Path = image.FileName,
                            Stream = image.OpenReadStream(),
                        });
                    }
                }
                createEvent.OrganizerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                Role creatorRole = Enum.Parse<Role>(User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value);

                await _createEventUseCase.ExecuteAsync(createEvent, creatorRole, avatarStream, additionalImages);

                // Dispose opening streams
                avatarStream?.Dispose();
                foreach (var img in additionalImages) img.Stream?.Dispose();
                TempData[ToastType.SuccessMessage.ToString()] = "Create event successfully";
            }
            catch
            {
                TempData[ToastType.ErrorMessage.ToString()] = "Create event failed";
                //_logger.
            }
            return RedirectToAction(nameof(Create));
        }

        // GET: EventRegistrationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EventRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventRegistrationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EventRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
