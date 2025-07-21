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
        Task<Event> AddAsync(Event createEvent);
    }
}
