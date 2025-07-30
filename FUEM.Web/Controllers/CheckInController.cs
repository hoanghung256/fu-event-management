using FUEM.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System;
using OpenCvSharp;
using FUEM.Infrastructure.Persistence;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using FUEM.Domain.Entities;
using System.Text.Json;
using FUEM.Infrastructure.Common.FaceRecognization;
using FUEM.Application.Interfaces.StudentUseCases;

namespace FUEM.Web.Controllers
{
    public class CheckInController : Controller
    {
        private readonly ICheckInUseCase _checkInUseCase;
        private readonly IGetStudent _student;
        private readonly IGetEventForGuest _getEventForGuest;

        public CheckInController(ICheckInUseCase checkInUseCase, IGetStudent student, IGetEventForGuest getEventForGuest)
        {
            _checkInUseCase = checkInUseCase;
            _student = student;
            _getEventForGuest = getEventForGuest;
        }

        public async Task<IActionResult> Index(int eventId)
        {
            Console.WriteLine($"id {eventId}");
            Event e = await _getEventForGuest.GetEventByIdAsync(eventId);
            return View(e);
        }

        public IActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyFace([FromBody] FaceInput input)
        {
            try
            {
                Student? s = await _checkInUseCase.GetStudentByFaceAsync(input);

                if (s != null)
                {
                    return Json(new { 
                        success = true,
                        message = $"✅ Welcome {s.Fullname}!",
                        student = new
                        {
                            id = s.Id,
                            fullname = s.Fullname,
                            email = s.Email,
                            avatarPath = s.AvatarPath
                        }
                    });
                }
                return Json(new { 
                    success = true,
                    message = "❌ Not recognized, please try again!" 
                });
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false,
                    message = $"Error: {ex.Message}" 
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCheckIn(int eventId, int studentId)
        {
            try
            {
                var result = await _checkInUseCase.CheckInAsync(eventId, studentId);
                if (result == true)
                {
                    TempData[ToastType.SuccessMessage.ToString()] = "✅ Check-in successful!";
                }
                else
                {
                    TempData[ToastType.ErrorMessage.ToString()] = "⚠️ Already checked in or invalid request!";
                }
            }
            catch (Exception ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = $"❌ Error: {ex.Message}";
            }
            return RedirectToAction("Index", "CheckIn", new { eventId = eventId });
        }

    }
}
