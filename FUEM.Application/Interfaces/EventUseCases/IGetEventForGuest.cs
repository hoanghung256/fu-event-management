using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.EventUseCases
{
    public interface IGetEventForGuest
    {
        Task<IEnumerable<Event>> GetEventForGuestAsync();
    }
}
