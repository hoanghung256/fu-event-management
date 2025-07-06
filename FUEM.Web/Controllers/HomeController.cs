using Firebase.Storage;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Infrastructure.Common;
using FUEM.Infrastructure.Common.MailSender;
using FUEM.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client.Extensions.Msal;

//using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FUEM.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetEventForGuest _getEventForGuestUseCase;
        private readonly FirebaseStorageService _storage;

        public HomeController(ILogger<HomeController> logger, IGetEventForGuest getEventForGuestUseCase, FirebaseStorageService storage)
        {
            _logger = logger;
            _getEventForGuestUseCase = getEventForGuestUseCase;
            _storage = storage;
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
            string fileName = "image/app/decorate/check-in-page.jpg";
            var url = await _storage.GetSignedFileUrlAsync(fileName);
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
                        await _storage.UploadFileAsync(FileType.Image, stream, uploadedFile.Name);
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

        [HttpPost]
        public async Task<IActionResult> SendMail()
        {
            try
            {
                //await MailSender.SendOTPAsync("hoanghung250604@gmail.com", "1234");
                await MailSender.SendOTPAsync("khiem30042004@gmail.com", "1234");
                TempData[ToastType.SuccessMessage.ToString()] = "Send mail success";
            }
            catch (Exception ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.Message;
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction(nameof(Test));
        }

    }
}
