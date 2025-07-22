using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentByEmailAsync(string email);

        Task AddAsync(Student s);
        Task UpdatePasswordHashAsync(int studentId, string newPasswordHash);
        Task<string?> GetPasswordHashAsync(int studentId);
        Task<bool> IsUserFollowingAsync(int studentId, int organizerId);
        Task FollowAsync(int studentId, int organizerId);
        Task UnfollowAsync(int studentId, int organizerId);
    }
}
