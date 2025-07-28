using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
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
        public async Task<Event?> GetEventByIdAsync(int eventId)
        {
            return await _context.Events
                .Include(e => e.Location)
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.EventImages)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<Page<Event>> GetEventForGuestAsync(int page, int pageSize)
        {
            int totalItems = await _context.Events.CountAsync();

            var items = await _context.Events
                .Include(e => e.Location)
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.EventImages)
                .Where(e => e.Status == EventStatus.APPROVED || e.Status == EventStatus.ON_GOING)
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

        public async Task<Page<Event>> GetPendingEventForAdmin(int page, int pageSize)
        {
            //int totalItems = await _context.Events.CountAsync(e => e.Status == EventStatus.PENDING);
            var query = _context.Events
                .AsNoTracking()
                .Include(e => e.Location)
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.EventImages)
                .Where(e => e.Status == EventStatus.PENDING)
                .OrderByDescending(e => e.DateOfEvent)
                .AsQueryable();

            int totalItems = await query.CountAsync();

            var items = await query
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
        public async Task<List<Event>> GetRegisteredEventsForStudentAsysnc(int studentId, DateTime startDate, DateTime endDate)
        {
            DateOnly startOnly = DateOnly.FromDateTime(startDate);
            DateOnly endOnly = DateOnly.FromDateTime(endDate);
            var events = await _context.Events
                .Include(e => e.EventGuests)
                .Include(e => e.Category)
                .Include(e => e.Organizer)
                .Include(e => e.Location)
                .Include(e => e.EventImages)
                .Where(e => e.EventGuests.Any(eg => eg.GuestId == studentId) && e.DateOfEvent >= startOnly && e.DateOfEvent <= endOnly)
                .ToListAsync();
            return events;
        }

        public async Task<Page<Event>> SearchEventAsync(SearchEventCriteria criteria, int page, int pageSize)
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
        
        public async Task<Page<Event>> GetAllOrganizedEventsAsync(int pageNumber, int pageSize)
        {
            int totalItems = await _context.Events
                .Where(e => e.Status == EventStatus.END)
                .CountAsync();

            var items = await _context.Events
                .Where(e => e.Status == EventStatus.END)
                .OrderByDescending(e => e.DateOfEvent)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Page<Event>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        
        public async Task<Page<Event>> GetOrganizedEventsByOrganizerIdAsync(string organizerId, int pageNumber, int pageSize)
        {
            int totalItems = await _context.Events
                .Where(e => e.OrganizerId.ToString() == organizerId && e.Status == EventStatus.END)
                .CountAsync();

            var items = await _context.Events
                .Where(e => e.OrganizerId.ToString() == organizerId && e.Status == EventStatus.END)
                .OrderByDescending(e => e.DateOfEvent)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Page<Event>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<Page<Event>> GetAttendedEventsForUserIdAsync(string userId, int pageNumber, int pageSize)
        {
            int guestId = int.Parse(userId);

            int totalItems = await _context.EventGuests
                .Where(eg => eg.GuestId == guestId && eg.IsAttended == true)
                .CountAsync();

            var eventIds = await _context.EventGuests
                .Where(eg => eg.GuestId == guestId && eg.IsAttended == true)
                .OrderByDescending(eg => eg.EventId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(eg => eg.EventId)
                .ToListAsync();

            var events = await _context.Events
                .Include(e => e.Location) 
                .Where(e => eventIds.Contains(e.Id))
                .ToListAsync();

            return new Page<Event>
            {
                Items = events,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }



        public async Task<bool> UpdateEventAsync(Event e)
        {
            _context.Events.Update(e);
            int rowAffected = await _context.SaveChangesAsync();

            return rowAffected > 0;
        }

        public async Task<List<Event>> GetRecentEventsByOrganizerId(int organizerId, int count = 10)
        {
            var result = await _context.Events
                .Where(e => e.OrganizerId == organizerId)
                .Include(e => e.Category)
                .Include(e => e.Location)
                .Include(e => e.EventImages)
                .OrderByDescending(e => e.DateOfEvent)
                .Take(count)
                .ToListAsync();
            return result;
        }
    }

}
