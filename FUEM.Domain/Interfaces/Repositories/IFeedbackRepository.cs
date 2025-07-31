using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface IFeedbackRepository
    {
        Task AddFeedbackAsync(Feedback feedback);
        Task<Feedback> GetFeedbackByGuestAndEventAsync(int guestId, int eventId); // Thêm để kiểm tra trùng lặp
        Task<bool> HasUserSubmittedFeedbackForEvent(int guestId, int eventId); // Thêm để kiểm tra sự tồn tại
        Task<IEnumerable<Feedback>> GetFeedbacksByEventIdAsync(int eventId);
    }
}
