
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FUEM.Application.Interfaces.EventUseCases
{
    public interface IGetAttendedEvents
    {
       
        Task<Page<Event>> GetAttendedEventsForUserAsync(string userId, int pageNumber, int pageSize);
    }
}
