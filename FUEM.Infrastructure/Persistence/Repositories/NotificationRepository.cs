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
    public class NotificationRepository : INotificationRepository
    {
        private readonly FUEMDbContext _context;

        public NotificationRepository(FUEMDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task AddNotificationReceiversAsync(IEnumerable<NotificationReceiver> receivers)
        {
            _context.NotificationReceivers.AddRange(receivers);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountNotificationsByReceiverIdAndTypeAsync(int receiverId, bool isOrganizer)
        {
            return await _context.NotificationReceivers
                                 .Where(nr => nr.ReceiverId == receiverId && nr.IsOrganizer == isOrganizer)
                                 .CountAsync();
        }

        public async Task<List<Notification>> GetNotificationsByReceiverIdAndTypeAsync(int receiverId, bool isOrganizer)
        {
            var notifications = await (from nr in _context.NotificationReceivers
                                       join n in _context.Notifications on nr.NotificationId equals n.Id
                                       where nr.ReceiverId == receiverId && nr.IsOrganizer == isOrganizer
                                       orderby n.SendingTime descending
                                       select n)
                          .Include(n => n.Sender)
                           .ToListAsync();
            return notifications;
        }
    }
}
