using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.UserUseCases
{
    public class Login : ILogin
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IOrganizerRepository _organizerRepository;

        public Login(IStudentRepository studentRepository, IOrganizerRepository organizerRepository)
        {
            _studentRepository = studentRepository;
            _organizerRepository = organizerRepository;
        }

        public async Task<object> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("Email/Password can not be null");
            }
            var stu = await _studentRepository.GetStudentByEmailAsync(email);
            if (stu != null)
            {
                if (stu.Password.Equals(password))
                    return stu;
                else
                    throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var org = await _organizerRepository.GetOrganizerByEmailAsync(email);
            if (org != null)
            {
                if (org.Password.Equals(password))
                    return org;
                else
                    throw new UnauthorizedAccessException("Invalid credentials.");
            }

            throw new UnauthorizedAccessException("Email not found.");
        }
    }
}

