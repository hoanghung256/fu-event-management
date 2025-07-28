using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.EventUseCases
{
    public interface IGetEventForGuest
    {
        Task<Page<Event>> GetEventForGuestAsync(int page = 1, int pageSize = 10);
        Task<Page<Event>> SearchEventAsync(SearchEventCriteria? criteria, int page = 1, int pageSize = 10);
        Task<Event?> GetEventByIdAsync(int id);
        Task<Page<Event>> GetUpcomingEventForAdminAsync(int page = 1, int pageSize = 10);
    }
}
