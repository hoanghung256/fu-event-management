using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Domain.Entities;

namespace FUEM.Application.Interfaces.NotificationUseCases
{
    public interface IStudentNotification
    {
        Task<List<Notification>> GetStudentNotificationsAsync(int studentId);
        Task<int> GetStudentNotificationCountAsync(int studentId);
    }
}
