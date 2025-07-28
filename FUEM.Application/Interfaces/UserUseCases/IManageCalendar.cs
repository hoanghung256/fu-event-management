using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Domain.Entities;

namespace FUEM.Application.Interfaces.UserUseCases
{
    public interface IManageCalendar
    {
        Task<List<Event>> GetRegisteredEventsForStudentAsync(int studentId, DateTime startDate, DateTime endDate);
    }
}
