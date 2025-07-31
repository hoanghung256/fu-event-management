using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface IEventGuestRepository
    {
        Task<bool> IsAlreadyRegisteredAsync(int eventId, int studentId);
        Task AddAsync(EventGuest guest);
        Task<EventGuest?> GetGuestByEventIdAsync(int eventId, int studentId);
        Task<bool> CheckInAsync(int eventId, int studentId);
    }

}
