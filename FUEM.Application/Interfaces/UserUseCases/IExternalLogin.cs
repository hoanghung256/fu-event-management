using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace FUEM.Application.Interfaces.UserUseCases
{
    public interface IExternalLogin
    {
        Task<object> HandleGoogleLoginAsync(string email, string fullName);
    }
}
