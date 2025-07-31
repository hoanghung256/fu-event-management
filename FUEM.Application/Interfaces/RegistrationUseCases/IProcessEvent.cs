using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.RegistrationUseCases
{
    public interface IProcessEvent
    {
        Task<Page<Event>> GetPendingEventForAdmin(int pageNumber, int pageSize);
        Task<Page<Event>> GetPendingEventOfOrganized(int organizerId, int pageNumber, int pageSize);
        public Task<bool> ProcessEventAsync(int eventId, string action);
    }
}
