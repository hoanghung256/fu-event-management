using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.StudentUseCases
{
    public interface ICheckInUseCase
    {
        Task SaveFaceEmbeddingAsync(List<FaceEmbedding> faceEmbeddingList);
        Task<Student?> GetStudentByFaceAsync(FaceInput faceInput);
        Task<bool?> CheckInAsync(int eventId, int studentId);
    }
}
