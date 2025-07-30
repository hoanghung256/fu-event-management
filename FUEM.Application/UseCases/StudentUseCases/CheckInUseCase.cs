using FUEM.Application.Interfaces.StudentUseCases;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common.FaceRecognization;
using FUEM.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.StudentUseCases
{
    public class CheckInUseCase : ICheckInUseCase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly FaceRecognizeService _faceRecognizeService;
        private readonly IFaceEmbeddingRepository _faceEmbeddingRepository;
        private readonly IEventGuestRepository _eventGuestRepository;

        public CheckInUseCase(IFaceEmbeddingRepository faceEmbeddingRepository,IStudentRepository studentRepository, FaceRecognizeService faceRecognizeService, IEventGuestRepository eventGuestRepository) 
        {
            _studentRepository = studentRepository;
            _faceRecognizeService = faceRecognizeService;
            _faceEmbeddingRepository = faceEmbeddingRepository;
            _eventGuestRepository = eventGuestRepository;
        }

        public async Task<Student?> GetStudentByFaceAsync(FaceInput faceInput)
        {
            string base64 = faceInput.ImageBase64.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(base64);
            Mat img = Cv2.ImDecode(imageBytes, ImreadModes.Color);

            var face = FacePreprocessor.CropAndResize(img);
            var inputData = FacePreprocessor.Normalize(face);

            var inputEmb = _faceRecognizeService.ExtractEmbedding(inputData);

            var students = await _studentRepository.GetAllStudentsForCheckInAsync();
            foreach (var s in students)
            {
                if (_faceRecognizeService.VerifyEmbedding(s.FaceEmbeddings.ToList(), inputEmb))
                {
                    return s;
                }
            }

            return null;
        }

        public Task SaveFaceEmbeddingAsync(List<FaceEmbedding> faceEmbeddingList) 
            => _faceEmbeddingRepository.SaveEmbeddingsAsync(faceEmbeddingList);

        public async Task<bool?> CheckInAsync(int eventId, int studentId)
            => await _eventGuestRepository.CheckInAsync(eventId, studentId);

    }
}
