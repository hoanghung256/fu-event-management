using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class EventGuestRepository : IEventGuestRepository
    {
        private readonly FUEMDbContext _context;
        public EventGuestRepository(FUEMDbContext context) => _context = context;

        public async Task<bool> IsAlreadyRegisteredAsync(int eventId, int studentId)
        {
            return await _context.EventGuests
                .AnyAsync(x => x.EventId == eventId && x.GuestId == studentId && x.IsRegistered == true);
        }

        public async Task AddAsync(EventGuest guest)
        {
            _context.EventGuests.Add(guest);
            await _context.SaveChangesAsync();
        }

        public async Task<EventGuest?> GetGuestByEventIdAsync(int eventId, int studentId)
            => await _context.EventGuests.FirstOrDefaultAsync(eg => eg.EventId == eventId && eg.GuestId == studentId);

        public async Task<bool> CheckInAsync(int eventId, int studentId)
        {
            var existingRecord = await GetGuestByEventIdAsync(eventId, studentId);

            if (existingRecord != null)
            {
                if (existingRecord.IsAttended == false)
                {
                    existingRecord.IsAttended = true;
                    _context.EventGuests.Update(existingRecord);
                    await _context.SaveChangesAsync();
                }
                return true; 
            }

            var newRecord = new EventGuest
            {
                EventId = eventId,
                GuestId = studentId,
                IsRegistered = false,
                IsAttended = true,
                IsCancelRegister = false,
            };

            _context.EventGuests.Add(newRecord);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
