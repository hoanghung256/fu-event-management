using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;

namespace FUEM.Application.UseCases.UserUseCases
{
    public class ManageCalendar : IManageCalendar
    {
        private readonly IEventRepository _eventRepository;
        public ManageCalendar(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<List<Event>> GetRegisteredEventsForStudentAsync(int studentId, DateTime startDate, DateTime endDate)
        {
            return await _eventRepository.GetRegisteredEventsForStudentAsysnc(studentId, startDate, endDate);
        }
    }
}
