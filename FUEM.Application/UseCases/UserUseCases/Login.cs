using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Email/Password can not be null");
            }
            var stu = await _studentRepository.GetStudentByEmailAsync(email);
            if (stu != null)
            {
                if (Hasher.Verify(password, stu.Password))
                    return stu;
            }

            var org = await _organizerRepository.GetOrganizerByEmailAsync(email);
            if (org != null)
            {
                if (Hasher.Verify(password, org.Password))
                    return org;
            }

            return null;
        }
    }
}

