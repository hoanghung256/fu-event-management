using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Security.Claims;
using System.Security.Principal;

namespace FUEM.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogin _loginUseCase;
        private readonly ISignUp _signUpUseCase;

        public AuthenticationController(ILogin loginUseCase, ISignUp signUpUseCase)
        {
            _loginUseCase = loginUseCase;
            _signUpUseCase = signUpUseCase;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            try
            {
                var user = await _loginUseCase.LoginAsync(email, password);
                if (user != null)
                {
                    
                    int userId = 0;
                    Role role = user is Student ? Role.Student : Role.Organizer;
                    HttpContext.Session.SetString("Email", email);
                    if (user is Student)
                    {
                        userId = ((Student)user).Id;
                        HttpContext.Session.SetString("Role", Role.Student.ToString()); 
                    }
                    else if (user is Organizer)
                    {
                        userId = ((Organizer)user).Id;
                        bool isAdmin = ((Organizer)user).IsAdmin ?? false;
                        role = isAdmin ? Role.Admin : Role.Organizer;
                        HttpContext.Session.SetString("Role", Role.Organizer.ToString());
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.Role, role.ToString())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync("Cookies", principal);
                    return RedirectToAction("Index", "Home");
                }

                TempData[ToastType.ErrorMessage.ToString()] = "Login failed.";
                return View();
            }
            catch (Exception ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.Message;
                return View(); 
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromForm]Student student)
        {
            try
            {
                await _signUpUseCase.ExecuteAsync(student);
                TempData[ToastType.SuccessMessage.ToString()] = "Sign up successful. You can now log in.";
                return View("Login");
            }
            catch (ArgumentException ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.Message;
                return View();
            }
        }
    }
}
