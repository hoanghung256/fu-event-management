using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;

namespace FUEM.Application.Interfaces.NotificationUseCases
{
    public interface IClubNotification
    {
        Task<int> GetClubNotificationCountAsync(int studentId);

        Task<List<Notification>> GetClubNotificationsAsync(int studentId);
        Task<Page<Event>> GetEventsByOrganizerIdAsync(int organizerId, int page, int pageSize);
        Task<Notification> SendAndSaveNotificationAsync(int senderId, string title, string message, IEnumerable<int> receiverIds, bool isOrganizerReceivers);
        Task<List<int>> GetStudentIdsByEventIdAsync(int eventId);
    }
}
