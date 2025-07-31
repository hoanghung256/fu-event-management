using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;

namespace FUEM.Application.Interfaces.NotificationUseCases
{
    public interface IAdminNotification
    {
        Task<List<int>> GetStudentIdsByEventIdAsync(int eventId);
        Task<List<int>> GetAllStudentIdsAsync();
        Task<List<int>> GetClubPresidentIdsAsync();
        Task<Page<Event>> GetEventsByOrganizerIdAsync(int organizerId, int page, int pageSize);
        Task<Notification> SendAndSaveNotificationAsync(int senderId, string title, string message, IEnumerable<int> receiverIds, bool isOrganizerReceivers);

    }
}
