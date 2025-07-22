using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.StudentUseCases
{
    public interface IGetStudent
    {
        Task<Student> GetStudentByEmail(string email);
    }
}
