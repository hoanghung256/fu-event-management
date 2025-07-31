using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly FUEMDbContext _context;

        public StudentRepository(FUEMDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student s)
        {
            await _context.Students.AddAsync(s);
            await _context.SaveChangesAsync();
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
            => await _context.Students.FirstOrDefaultAsync(s => s.Email == email);

        public async Task<Student?> GetStudentByIdAsync(int id)
            => await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

        public async Task<string?> GetPasswordHashAsync(int studentId)
        {
            return (await _context.Students.FindAsync(studentId))?.Password;
        }

        public async Task UpdatePasswordHashAsync(int studentId, string newPasswordHash)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                student.Password = newPasswordHash;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserFollowingAsync(int studentId, int organizerId)
        {
            return await _context.Students
                .Where(s => s.Id == studentId)
                .AnyAsync(s => s.Organizers.Any(o => o.Id == organizerId));
        }

        public async Task FollowAsync(int studentId, int organizerId)
        {
            var student = await _context.Students
                .Include(s => s.Organizers)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            var organizer = await _context.Organizers.FindAsync(organizerId);

            if (student != null && organizer != null && !student.Organizers.Contains(organizer))
            {
                student.Organizers.Add(organizer);
                organizer.FollowerCount = (organizer.FollowerCount ?? 0) + 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnfollowAsync(int studentId, int organizerId)
        {
            var student = await _context.Students
                .Include(s => s.Organizers)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            var organizer = await _context.Organizers.FindAsync(organizerId);

            if (student != null && organizer != null && student.Organizers.Contains(organizer))
            {
                student.Organizers.Remove(organizer);
                if (organizer.FollowerCount > 0)
                    organizer.FollowerCount--;
                await _context.SaveChangesAsync();
            }
        }

        public Task<List<Student>> GetAllStudentsForCheckInAsync() => _context.Students.Include(s => s.FaceEmbeddings).ToListAsync();
    }
}
