using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FUEMDbContext _context;

        public FeedbackRepository(FUEMDbContext context)
        {
            _context = context;
        }
        public async Task AddFeedbackAsync(Feedback feedback)
        {
            
            var existingFeedback = await _context.Feedbacks
                                                .FirstOrDefaultAsync(f => f.GuestId == feedback.GuestId && f.EventId == feedback.EventId);

            if (existingFeedback != null)
            {
                throw new ArgumentException("Feedback has been submitted.");
            }

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync(); 
        }

        public async Task<Feedback> GetFeedbackByGuestAndEventAsync(int guestId, int eventId)
        {
            return await _context.Feedbacks
                                 .FirstOrDefaultAsync(f => f.GuestId == guestId && f.EventId == eventId);
        }

        public async Task<bool> HasUserSubmittedFeedbackForEvent(int guestId, int eventId)
        {
            return await _context.Feedbacks
                                 .AnyAsync(f => f.GuestId == guestId && f.EventId == eventId);
        }
    
}
}
