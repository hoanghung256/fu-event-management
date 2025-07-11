using Microsoft.AspNetCore.Mvc;

namespace FUEM.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View("Test");
        }
    }
}
