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
    public class EventRepository : IEventRepository
    {
        private readonly FUEMDbContext _context;

        public EventRepository(FUEMDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Event>> GetEventForGuestAsync()
        {
            return await _context.Events.Take(10).ToListAsync();
        }

        public Task RemoveAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public Task<Event> UpdateAsync(Event entity)
        {
            throw new NotImplementedException();
        }
    }
}
