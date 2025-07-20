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
        Task<Page<Event>> SearchEventAsync(SearchEventCriteria criteria, int page, int pageSize);
        Task<List<Event>> GetRecentEventsByOrganizerId(int organizerId, int count = 10);
        Task<Event> AddAsync(Event createEvent);
        Task<Page<Event>> GetPendingEventForAdmin(int page, int pageSize);
        Task<Event?> GetEventByIdAsync(int eventId);
        Task<bool> UpdateEventAsync(Event e);
    }
}
