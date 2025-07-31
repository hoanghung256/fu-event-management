using Firebase.Storage;
using FUEM.Application.Interfaces.CategoryUseCases;
using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using FUEM.Infrastructure.Common.MailSender;
using FUEM.Infrastructure.Persistence.Repositories;
using FUEM.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Diagnostics.Tracing;
using System.Security.Claims;


namespace FUEM.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetEventForGuest _getEventForGuestUseCase;
        private readonly IGetAllCategories _getAllCategoriesUseCase;
        private readonly IGetAllOrganizers _getAllOrganizersUseCase;
        private readonly FirebaseStorageService _storage;

        public HomeController(ILogger<HomeController> logger, IGetEventForGuest getEventForGuestUseCase, IGetAllCategories getAllCategoriesUseCase, IGetAllOrganizers getAllOrganizersUseCase, FirebaseStorageService storage)
        {
            _logger = logger;
            _getEventForGuestUseCase = getEventForGuestUseCase;
            _getAllCategoriesUseCase = getAllCategoriesUseCase;
            _getAllOrganizersUseCase = getAllOrganizersUseCase;
            _storage = storage;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromQuery] SearchEventCriteria? criteria, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, bool isFind = false)
        {
            try
            {
                Page<Event> eventsPage = isFind
                ? await _getEventForGuestUseCase.SearchEventAsync(criteria, pageNumber, pageSize)
                : await _getEventForGuestUseCase.GetEventForGuestAsync(pageNumber, pageSize);

                var viewModel = new EventListViewModel
                {
                    Items = eventsPage.Items,
                    CurrentPage = eventsPage.PageNumber,
                    TotalPages = eventsPage.TotalPages,
                    SearchCriteria = criteria
                };

                ViewData["Categories"] = await _getAllCategoriesUseCase.GetAllCategoriesAsync();
                ViewData["Organizers"] = await _getAllOrganizersUseCase.GetAllOrganizersAsync();
                ViewData["PreviousSearchEventCriteria"] = criteria;
                ViewData["IsSearch"] = isFind;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.Message;
                return View();
            }
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
                        await _storage.UploadFileAsync(FileType.Image, stream, uploadedFile.FileName);
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
