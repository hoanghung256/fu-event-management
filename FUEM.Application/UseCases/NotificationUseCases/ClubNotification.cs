using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Application.Interfaces.NotificationUseCases;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Persistence.Repositories;

namespace FUEM.Application.UseCases.NotificationUseCases
{
    internal class ClubNotification : IClubNotification
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly INotificationReceiverRepository _notificationReceiverRepository;
        public ClubNotification(INotificationRepository notificationRepository, IEventRepository eventRepository, INotificationReceiverRepository notificationReceiverRepository)
        {
            _notificationRepository = notificationRepository;
            _eventRepository = eventRepository;
            _notificationReceiverRepository = notificationReceiverRepository;
        }
        public async Task<int> GetClubNotificationCountAsync(int clubId)
        {
            return await _notificationRepository.CountNotificationsByReceiverIdAndTypeAsync(clubId, true);
        }

        public async Task<List<Notification>> GetClubNotificationsAsync(int clubId)
        {
            var notifications = await _notificationRepository.GetNotificationsByReceiverIdAndTypeAsync(clubId, true);

            foreach (var notification in notifications)
            {
                if (string.IsNullOrEmpty(notification.Title))
                {
                    notification.Title = "Notification without title";
                }
            }
            return notifications;
        }

        public Task<Page<Event>> GetEventsByOrganizerIdAsync(int organizerId, int page, int pageSize)
        {
            return _eventRepository.GetEventsByOrganizerIdAsync(organizerId, page, pageSize);
        }

        public async Task<Notification> SendAndSaveNotificationAsync(int senderId, string title, string message, IEnumerable<int> receiverIds, bool isOrganizerReceivers)
        {
            var notification = new Notification
            {
                SenderId = senderId,
                Title = title,
                Content = message,
                SendingTime = DateTime.UtcNow
            };
            notification = await _notificationRepository.AddNotificationAsync(notification);

            if (notification == null)
            {
                return null;
            }

            var receivers = receiverIds.Select(id => new NotificationReceiver
            {
                NotificationId = notification.Id,
                ReceiverId = id,
                IsOrganizer = isOrganizerReceivers
            }).ToList();

            if (receivers.Any())
            {
                await _notificationRepository.AddNotificationReceiversAsync(receivers);
            }
            return notification;
        }

        public async Task<List<int>> GetStudentIdsByEventIdAsync(int eventId)
        {
            return await _notificationReceiverRepository.GetStudentIdsByEventIdAsync(eventId);
        }
    }
}
