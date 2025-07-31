using FUEM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class NotificationReceiverRepository : INotificationReceiverRepository
    {
        private readonly FUEMDbContext _context;

        public NotificationReceiverRepository(FUEMDbContext context)
        {
            _context = context;
        }

        public async Task<List<int>> GetAllStudentIdsAsync()
        {
            return await _context.Students.Select(s => s.Id).ToListAsync();
        }

        public async Task<List<int>> GetClubPresidentIdsAsync()
        {
            return await _context.Organizers.Where(o => o.IsAdmin == false).Select(o => o.Id).ToListAsync();
        }

        public async Task<List<int>> GetStudentIdsByEventIdAsync(int eventId)
        {
            return await _context.EventGuests.Where(eg => eg.EventId == eventId).Select(eg => eg.GuestId).Distinct().ToListAsync();
        }
    }
}
