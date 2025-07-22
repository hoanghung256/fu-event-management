using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.EventUseCases
{
    public interface IGetRecentEvents
    {
        Task<List<Event>> GetRecentEventsByOrganizerId(int organizerId, int count = 10);
    }
}
