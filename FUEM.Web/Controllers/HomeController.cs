using FUEM.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Index()
        {
            var events = await _getEventForGuestUseCase.GetEventForGuestAsync();
            TempData[ToastType.InfoMessage.ToString()] = "Welcome to FUEM!";
            return View(new EventListViewModel() { Items = events });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
