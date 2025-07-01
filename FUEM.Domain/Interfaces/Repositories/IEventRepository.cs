using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Page<Event>> GetEventForGuestAsync(int page, int pageSize);
    }
}
