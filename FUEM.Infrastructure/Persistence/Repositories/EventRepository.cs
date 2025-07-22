using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public async Task<Event> AddAsync(Event createEvent)
        {
            _context.Events.Add(createEvent);
            await _context.SaveChangesAsync();
            return createEvent;
        }

        public async Task<Event> GetEventById(int id)
        {
            var detail = await _context.Events
                .Include(e => e.EventImages)
                .Include(e => e.Location)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == id);

            return detail;
        }
        public async Task<Page<Event>> GetEventForGuestAsync(int page, int pageSize)
        {
            int totalItems = await _context.Events.CountAsync();

            var items = await _context.Events
                .Include(e => e.Location)
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.EventImages)
                .OrderByDescending(e => e.DateOfEvent)
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

        public async Task<Page<Event>> SearchEventAsync(SearchEventCriteria? criteria, int page, int pageSize)
        {
            var query = _context.Events.Include(e => e.Category)
                                       .Include(e => e.Organizer)
                                       .Include(e => e.Location)
                                       .Include(e => e.EventImages)
                                       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(criteria.Name))
            {
                query = query.Where(e => e.Fullname!.ToLower().Contains(criteria.Name.ToLower()));
            }

            if (criteria.CategoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == criteria.CategoryId.Value);
            }

            if (criteria.OrganizerId.HasValue)
            {
                query = query.Where(e => e.OrganizerId == criteria.OrganizerId.Value);
            }

            if (criteria.FromDate.HasValue)
            {
                query = query.Where(e => e.DateOfEvent >= criteria.FromDate);
            }

            if (criteria.ToDate.HasValue)
            {
                query = query.Where(e => e.DateOfEvent <= criteria.ToDate);
            }

            var total = await query.CountAsync();
            var items = await query.OrderByDescending(e => e.DateOfEvent)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new Page<Event>
            {
                Items = items,
                TotalItems = total,
                PageNumber = page,
                PageSize = pageSize
            };
        }
    }
}
