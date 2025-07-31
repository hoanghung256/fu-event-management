using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Application.Interfaces.NotificationUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;

namespace FUEM.Application.UseCases.NotificationUseCases
{
    public class StudentNotification : IStudentNotification
    {
        private readonly INotificationRepository _notificationRepository;
        public StudentNotification(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<int> GetStudentNotificationCountAsync(int studentId)
        {
            return await _notificationRepository.CountNotificationsByReceiverIdAndTypeAsync(studentId, false);
        }

        public async Task<List<Notification>> GetStudentNotificationsAsync(int studentId)
        {
            var notifications = await _notificationRepository.GetNotificationsByReceiverIdAndTypeAsync(studentId, false);

            foreach (var notification in notifications)
            {
                if (string.IsNullOrEmpty(notification.Title))
                {
                    notification.Title = "Notification without title";
                }
            }
            return notifications;
        }
    }
}
