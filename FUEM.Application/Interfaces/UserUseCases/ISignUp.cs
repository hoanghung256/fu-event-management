using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.UserUseCases
{
    public interface ISignUp
    {
        Task ExecuteAsync(Student s);
    }
}
