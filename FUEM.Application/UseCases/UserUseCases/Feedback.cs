using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.UserUseCases
{
    public class FeedbackService : IFeedback 
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task AddFeedbackAsync(Feedback feedback)
        {
       
            await _feedbackRepository.AddFeedbackAsync(feedback);
        }

        public async Task<bool> CheckUserFeedbackForEventAsync(int guestId, int eventId)
        {
            return await _feedbackRepository.HasUserSubmittedFeedbackForEvent(guestId, eventId);
        }
    }
}
