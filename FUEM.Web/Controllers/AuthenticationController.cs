using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FUEM.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogin _loginUseCase;
        private readonly ISignUp _signUpUseCase;
        private readonly IForgotPassword _forgotPasswordUseCase;

        public AuthenticationController(ILogin loginUseCase, ISignUp signUpUseCase, IForgotPassword forgotPassword)
        {
            _loginUseCase = loginUseCase;
            _signUpUseCase = signUpUseCase;
            _forgotPasswordUseCase = forgotPassword;
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
                    Role? role = Role.Student;
                    HttpContext.Session.SetString("Email", email);
                    if (user is Student)
                    {
                        userId = ((Student)user).Id;
                        //HttpContext.Session.SetString("Role", Role.Student.ToString()); 
                    }
                    else if (user is Organizer)
                    {
                        userId = ((Organizer)user).Id;
                        bool isAdmin = ((Organizer)user).IsAdmin ?? false;
                        role = isAdmin ? Role.Admin : Role.Club;
                        //HttpContext.Session.SetString("Role", role.ToString());
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
                    TempData[ToastType.InfoMessage.ToString()] = $"Login Successfully";
                    return RedirectToAction("Index", "Home");
                }

                TempData[ToastType.ErrorMessage.ToString()] = $"Incorrect email or password. Please try again!";
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
        public async Task<IActionResult> SignUp([FromForm] Student student)
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromForm] string email)
        {
            // Manual validation
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "Email is required.");
                return View("ForgotPassword", (object)email);
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ModelState.AddModelError("email", "Invalid email address.");
                return View("ForgotPassword", (object)email);
            }

            try
            {
                string generatedOtp = await _forgotPasswordUseCase.RequestPasswordResetByEmailAsync(email);

                HttpContext.Session.SetString("Otp", generatedOtp);
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("ResetPasswordOtpExpiry", DateTime.UtcNow.AddMinutes(5).ToString("o"));

                TempData[ToastType.SuccessMessage.ToString()] = "An OTP has been sent to your email. Please check your inbox.";
                return RedirectToAction("ResetPassword");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("ForgotPassword", (object)email);
            }
            catch (Exception ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.Message;
                return View("ForgotPassword", (object)email);
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            string? email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                TempData[ToastType.ErrorMessage.ToString()] = "You need to enter an email to reset the password.";
                return RedirectToAction("ForgotPassword");
            }

            string? otpFromSession = HttpContext.Session.GetString("Otp");
            if (string.IsNullOrEmpty(otpFromSession))
            {
                TempData[ToastType.ErrorMessage.ToString()] = "OTP has expired or is invalid.";
                return RedirectToAction("ForgotPassword");
            }

            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            string? email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                TempData[ToastType.ErrorMessage.ToString()] = "You need to enter an email to reset the password.";
                return RedirectToAction("ForgotPassword");
            }

            model.Email = email;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? otpFromSession = HttpContext.Session.GetString("Otp");
            if (string.IsNullOrEmpty(otpFromSession))
            {
                TempData[ToastType.ErrorMessage.ToString()] = "OTP has expired or is invalid.";
                return View(model);
            }

            DateTime otpExiryTime;
            string? sessionOtpExpiryStr = HttpContext.Session.GetString("ResetPasswordOtpExpiry");
            if (!DateTime.TryParse(sessionOtpExpiryStr, out otpExiryTime) || otpExiryTime < DateTime.UtcNow)
            {
                HttpContext.Session.Remove("Otp");
                HttpContext.Session.Remove("ResetPasswordOtpExpiry");
                ModelState.AddModelError(string.Empty, "OTP has expired or is invalid. Please request a new OTP.");
                return View(model);
            }

            try
            {
                bool success = await _forgotPasswordUseCase.ResetPasswordAsync(
                    email,
                    model.Otp,
                    model.NewPassword,
                    otpFromSession
                );

                if (success)
                {
                    HttpContext.Session.Remove("Otp");
                    HttpContext.Session.Remove("ResetPasswordOtpExpiry");
                    HttpContext.Session.Remove("Email");
                    TempData[ToastType.SuccessMessage.ToString()] = "Password has been successfully reset. You can now log in with your new password.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Password reset failed. Please try again.");
                    return View(model);
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.Message;
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
