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
    }
}
