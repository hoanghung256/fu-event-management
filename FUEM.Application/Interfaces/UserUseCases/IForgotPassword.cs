using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.UserUseCases
{
    public interface IForgotPassword
    {
        Task<string> RequestPasswordResetByEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string otpInput, string newPassword, string? otpFromSession);

    }
}
