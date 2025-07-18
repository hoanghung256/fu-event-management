using FUEM.Application.Interfaces.CategoryUseCases;
using FUEM.Application.Interfaces.LocationUseCases;
using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FUEM.Web.Controllers
{
    [Authorize(Roles = "Admin, Club")]
    public class EventRegistrationController : Controller
    {
        private readonly ILogger<EventRegistrationController> _logger;
        private readonly ICreateEvent _createEventUseCase;
        private readonly IGetAllCategories _getAllCategoriesUseCase;
        private readonly IGetAllLocation _getAllLocationUseCase;

        public EventRegistrationController(ILogger<EventRegistrationController> logger, ICreateEvent createEventUseCase, IGetAllCategories getAllCategoriesUseCase, IGetAllOrganizers getAllOrganizersUseCase, IGetAllLocation getAllLocationUseCase)
        {
            _logger = logger;
            _createEventUseCase = createEventUseCase;
            _getAllCategoriesUseCase = getAllCategoriesUseCase;
            _getAllLocationUseCase = getAllLocationUseCase;
        }

        // GET: EventRegistrationController
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewData["Locations"] = await _getAllLocationUseCase.ExecuteAsync();
            ViewData["Categories"] = await _getAllCategoriesUseCase.GetAllCategoriesAsync();
            return View();
        }

        // POST: EventRegistrationController/Create
        [HttpPost]
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
