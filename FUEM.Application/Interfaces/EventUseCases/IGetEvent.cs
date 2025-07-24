using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.EventUseCases
{
    public interface IGetEvent
    {
        Task<Event?> GetEventByName(string eventName);
        Task<Event?> GetEventById(int eventId);
    }
}
