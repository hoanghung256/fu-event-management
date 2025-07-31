using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Application.Interfaces.NotificationUseCases;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;

namespace FUEM.Application.UseCases.NotificationUseCases
{
    public class AdminNotification : IAdminNotification
    {
        private readonly INotificationReceiverRepository _notificationReceiverRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEventRepository _eventRepository;
        public AdminNotification(INotificationReceiverRepository notificationReceiverRepository, IEventRepository eventRepository, INotificationRepository notificationRepository)
        {
            _notificationReceiverRepository = notificationReceiverRepository;
            _eventRepository = eventRepository;
            _notificationRepository = notificationRepository;
        }
        public async Task<List<int>> GetAllStudentIdsAsync()
        {
            return await _notificationReceiverRepository.GetAllStudentIdsAsync();
        }

        public async Task<List<int>> GetClubPresidentIdsAsync()
        {
            return await _notificationReceiverRepository.GetClubPresidentIdsAsync();
        }

        public Task<Page<Event>> GetEventsByOrganizerIdAsync(int organizerId, int page, int pageSize)
        {
            return _eventRepository.GetEventsByOrganizerIdAsync(organizerId, page, pageSize);
        }

        public async Task<List<int>> GetStudentIdsByEventIdAsync(int eventId)
        {
            return await _notificationReceiverRepository.GetStudentIdsByEventIdAsync(eventId);
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

            if(notification == null)
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
    }
}
