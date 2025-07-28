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

        public CheckInController(ICheckInUseCase checkInUseCase)
        {
            _checkInUseCase = checkInUseCase;
        }

        public IActionResult Index(int eventId) => View();

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
    }
}
