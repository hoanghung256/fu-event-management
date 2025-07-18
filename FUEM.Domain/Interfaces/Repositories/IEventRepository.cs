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

        Task<Event> AddAsync(Event createEvent);
    }

    public class SearchEventCriteria
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public int? OrganizerId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
