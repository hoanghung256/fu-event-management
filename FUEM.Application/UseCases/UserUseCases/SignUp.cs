using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using FUEM.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.UserUseCases
{
    internal class SignUp : ISignUp
    {
        private readonly int _studentIdLength = 8;
        private readonly IStudentRepository _studentRepository;

        public SignUp(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task ExecuteAsync(Student s)
        {
            if (!Verify(s.StudentId, s.Email)) throw new ArgumentException("Invalid student ID or email format.");
            Student? foundStudent = await _studentRepository.GetStudentByEmailAsync(s.Email);
            if (foundStudent != null) throw new ArgumentException("A student with this email already exists.");

            s.Password = Hasher.Hash(s.Password);

            await _studentRepository.AddAsync(s);
        }

        private bool Verify(string studentId, string email)
        {
            string part = studentId.Split('@')[0];
            string idFromEmail = part.Substring(part.Length - _studentIdLength, part.Length);
            Console.WriteLine(idFromEmail);

            if (string.Compare(idFromEmail, studentId, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return false;
            }

            return true;
        }
    }
}
