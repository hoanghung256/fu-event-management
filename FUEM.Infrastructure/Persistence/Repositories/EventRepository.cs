using FUEM.Domain.Common;
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

        public async Task<Page<Event>> GetEventForGuestAsync(int page, int pageSize)
        {
            int totalItems = await _context.Events.CountAsync();
            var items = await _context.Events.OrderByDescending(e => e.DateOfEvent)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Page<Event>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };
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
