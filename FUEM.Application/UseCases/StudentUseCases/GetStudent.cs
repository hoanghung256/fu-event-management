using FUEM.Application.Interfaces.StudentUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.StudentUseCases
{
    public class GetStudent : IGetStudent
    {
        private readonly IStudentRepository _repository;

        public GetStudent(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Student> GetStudentByEmail(string email)
            => await _repository.GetStudentByEmailAsync(email);
    }
}
