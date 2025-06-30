using Firebase.Storage;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Infrastructure.Common;
using FUEM.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FUEM.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetEventForGuest _getEventForGuestUseCase;

        public HomeController(ILogger<HomeController> logger, IGetEventForGuest getEventForGuestUseCase)
        {
            _logger = logger;
            _getEventForGuestUseCase = getEventForGuestUseCase;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] int pageNumber = 1)
        {
            Page<Event> eventsPage = await _getEventForGuestUseCase.GetEventForGuestAsync(pageNumber, 10);
            TempData[ToastType.InfoMessage.ToString()] = "Welcome to FUEM!";
            return View(eventsPage);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            var firebase = new FirebaseStorageService();
            string fileName = "image/app/decorate/check-in-page.jpg";
            var url = await firebase.GetSignedFileUrlAsync(fileName);
            ViewBag.FileUrl = url;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Test(IFormFile uploadedFile)
        {
            try
            {
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    using (var stream = uploadedFile.OpenReadStream())
                    {
                        FirebaseStorageService firebase = new();
                        await firebase.UploadFileAsync(FileType.Image, stream, uploadedFile.Name);
                    }

                    TempData[ToastType.SuccessMessage.ToString()] = "File uploaded successfully!";
                }
                else
                {
                    TempData[ToastType.ErrorMessage.ToString()] = "No file selected.";
                }
            }
            catch (FirebaseStorageException ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.ResponseData;
            }
            return RedirectToAction("Test");
        }
    }
}
