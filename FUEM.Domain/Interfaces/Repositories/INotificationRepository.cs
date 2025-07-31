using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Domain.Entities;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> AddNotificationAsync(Notification notification);
        //Task AddNotificationReceiverAsync(NotificationReceiver receiver);
        Task AddNotificationReceiversAsync(IEnumerable<NotificationReceiver> receivers);
        Task<List<Notification>> GetNotificationsByReceiverIdAndTypeAsync(int receiverId, bool isOrganizer);
        Task<int> CountNotificationsByReceiverIdAndTypeAsync(int receiverId, bool isOrganizer);

        // Nếu cần lấy thông tin Student hoặc Organizer riêng, có thể thêm phương thức ở đây
        //Task<bool> IsStudentExistAsync(int studentId);
        //Task<bool> IsOrganizerExistAsync(int organizerId);
    }
}
