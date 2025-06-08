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
        public async Task<IActionResult> Index()
        {
            var events = await _getEventForGuestUseCase.GetEventForGuestAsync();
            TempData[ToastType.InfoMessage.ToString()] = "Welcome to FUEM!";
            return View(new EventListViewModel() { Items = events });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
