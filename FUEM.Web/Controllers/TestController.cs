using FUEM.Application.Interfaces.StudentUseCases;
using FUEM.Domain.Entities;
using FUEM.Infrastructure.Common.FaceRecognization;
using FUEM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCvSharp;
using System.Security.Claims;
using System.Text.Json;

namespace FUEM.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly FaceRecognizeService _faceRecognizeService;
        private readonly ICheckInUseCase _checkInUseCase;

        public TestController(FaceRecognizeService faceRecognizeService, ICheckInUseCase checkInUseCase)
        {
            _faceRecognizeService = faceRecognizeService;
            _checkInUseCase = checkInUseCase;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Test([FromForm] IFormFileCollection FaceImage)
        {
            try
            {
                if (FaceImage == null || FaceImage.Count == 0)
                    return BadRequest("No file uploaded!");

                int studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                List<FaceEmbedding> faceEmbeddings = new List<FaceEmbedding>();
                foreach (var file in FaceImage)
                {
                    if (file.Length > 0)
                        faceEmbeddings.Add(await ProcessFace(studentId, file));
                }
                await _checkInUseCase.SaveFaceEmbeddingAsync(faceEmbeddings);
                TempData[ToastType.SuccessMessage.ToString()] = "Save face success!";
            } 
            catch (Exception ex)
            {
                TempData[ToastType.ErrorMessage.ToString()] = ex.Message;
            }

            return View();
        }

        private async Task<FaceEmbedding> ProcessFace(int studentId, IFormFile faceFile)
        {
            // Load bytes
            using var ms = new MemoryStream();
            await faceFile.CopyToAsync(ms);
            byte[] imageBytes = ms.ToArray();

            // Decode with OpenCV
            Mat img = Cv2.ImDecode(imageBytes, ImreadModes.Color);

            // Detect face
            var cascade = new CascadeClassifier("wwwroot/haarcascade_frontalface_default.xml");
            var gray = new Mat();
            Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);
            var faces = cascade.DetectMultiScale(gray);
            if (faces.Length == 0)
                throw new Exception("No face detected!");

            // Crop & Resize
            var face = new Mat(img, faces[0]);
            Cv2.Resize(face, face, new OpenCvSharp.Size(112, 112));

            // Normalize
            float[] inputData = FacePreprocessor.Normalize(face);

            // Extract embedding
            var emb = _faceRecognizeService.ExtractEmbedding(inputData);

            // Save to DB
            var embeddingJson = JsonSerializer.Serialize(emb);

            return new FaceEmbedding
            {
                StudentId = studentId,
                EmbeddingJson = embeddingJson
            };
        }
    }
}
