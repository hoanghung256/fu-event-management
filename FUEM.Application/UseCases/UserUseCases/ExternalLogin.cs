using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;

namespace FUEM.Application.UseCases.UserUseCases
{
    public class ExternalLogin : IExternalLogin
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IOrganizerRepository _organizerRepository;

        public ExternalLogin(IStudentRepository studentRepository, IOrganizerRepository organizerRepository)
        {
            _studentRepository = studentRepository;
            _organizerRepository = organizerRepository;
        }

        public async Task<object> HandleGoogleLoginAsync(string email, string fullName)
        {
            // 1. Search Student
            var existingStudent = await _studentRepository.GetStudentByEmailAsync(email);
            if (existingStudent != null)
            {
                return existingStudent; 
            }

            // 2. Search Organizer
            var existingOrganizer = await _organizerRepository.GetOrganizerByEmailAsync(email);
            if (existingOrganizer != null)
            {
                return existingOrganizer;
            }

            // 3. If not exist, tao moi luon cho nong
            var newStudent = new Student
            {
                Email = email,
                Fullname = fullName,
                AvatarPath = null,
                Password = null,
            };

            await _studentRepository.AddAsync(newStudent);
            return newStudent;
        }
    }
}
