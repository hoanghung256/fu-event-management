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
            string size = "";

            try
            {
                if (string.IsNullOrEmpty(input.ImageBase64))
                {
                    return Json(new { success = false, message = "Empty image data" });
                }

                var base64Data = input.ImageBase64.Split(',')[1];
                var imageBytes = Convert.FromBase64String(base64Data);
                var sizeInKb = imageBytes.Length / 1024.0;

                size = $"Image size received: {imageBytes.Length} bytes (~{sizeInKb:F2} KB)";
                Student? s = await _checkInUseCase.GetStudentByFaceAsync(input);

                if (s != null)
                {
                    return Json(new {
                        success = true,
                        message = $"✅ Welcome {s.Fullname}!",
                        size = size,
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
                    message = "❌ Not recognized, please try again!",
                    size = size
                });
            }
            catch (Exception ex)
            {
                return Json(new {
                    success = false,
                    message = $"Error: {ex.Message}",
                    stackTrace = ex.StackTrace,
                    size = size
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCheckIn([FromBody] CheckInRequestViewModel requestBody)
        {
            try
            {
                var result = await _checkInUseCase.CheckInAsync(requestBody.EventId, requestBody.StudentId);
                if (result == true)
                {
                    return Json(new
                    {
                        success = true,
                        toastType = ToastType.SuccessMessage.ToString(),
                        message = "✅ Check-in successful!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        toastType = ToastType.ErrorMessage.ToString(),
                        message = "⚠️ Already checked in or invalid request!"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    toastType = ToastType.ErrorMessage.ToString(),
                    message = $"❌ Error: {ex.Message}"
                });
            }
        }


    }
}
