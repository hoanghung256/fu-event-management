using FUEM.Application.UseCases.UserUseCases;
using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.UserUseCases
{
    public interface IFeedback
    {
        Task AddFeedbackAsync(Feedback feedback);
        Task<bool> CheckUserFeedbackForEventAsync(int guestId, int eventId);
       
    }
}

