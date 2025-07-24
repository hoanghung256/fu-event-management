using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task<Page<Event>> GetEventForGuestAsync(int page, int pageSize);
        Task<Page<Event>> GetOrganizedEventsByOrganizerIdAsync(string organizerId, int pageNumber, int pageSize);
        Task<Page<Event>> GetAllOrganizedEventsAsync(int pageNumber, int pageSize);
        Task<Page<Event>> SearchEventAsync(SearchEventCriteria criteria, int page, int pageSize);
        Task<Event> GetEventById(int id);
        Task<List<Event>> GetRecentEventsByOrganizerId(int organizerId, int count = 10);
        Task<Event> AddAsync(Event createEvent);
        Task<Page<Event>> GetAttendedEventsForUserIdAsync(string userId, int pageNumber, int pageSize);
        Task<Page<Event>> GetPendingEventForAdmin(int page, int pageSize);
        Task<Event?> GetEventByIdAsync(int eventId);
        Task<Event?> GetEventByNameAsync(string eventName);
        Task<bool> UpdateEventAsync(Event e);
    }
}
