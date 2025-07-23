using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.RegistrationUseCases
{
    public interface IRegisterIntoEvent
    {
        Task RegisterGuestAsync(int eventId, int studentId);
        Task RegisterCollaboratorAsync(int eventId, int studentId);
        Task<bool> CheckIfResgisterAsGuest(int eventId, int studentId);
        Task<bool> CheckIfResgisterAsCollaborator(int eventId, int studentId);
    }
}
