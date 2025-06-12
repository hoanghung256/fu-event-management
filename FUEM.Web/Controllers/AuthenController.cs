using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FUEM.Web.Controllers
{
    public class AuthenController : Controller
    {
        private readonly ILogin _loginUseCase;

        public AuthenController(ILogin loginUseCase)
        {
            _loginUseCase = loginUseCase;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool isOrganizer = false)
        {
            try
            {
                var user = await _loginUseCase.LoginAsync(email, password);
                if (user != null)
                {
                    HttpContext.Session.SetString("Email", email);
                    if (user is Student)
                    {
                        HttpContext.Session.SetString("Role", Role.Student.ToString()); 
                    }
                    else if (user is Organizer)
                    {
                        HttpContext.Session.SetString("Role", Role.Organizer.ToString());
                    }
                    return RedirectToAction("Index", "Home");
                }

                TempData["Error"] = "Login failed.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(); 
            }
        }

    }
}
