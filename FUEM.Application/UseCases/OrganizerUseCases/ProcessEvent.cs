using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Application.Interfaces.RegistrationUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Persistence.Repositories;

namespace FUEM.Application.UseCases.OrganizerUseCases
{
    public class ProcessEvent : IProcessEvent
    {
        private readonly IEventRepository _eventRepository;
        public ProcessEvent(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<bool> ProcessEventAsync(int eventId, string action)
        {
            Event? e = null;
            e = await _eventRepository.GetEventByIdAsync(eventId);

            if (e == null)
            {
                throw new ArgumentException("Event not found.");
            }
            if (action == "approve")
            {
                e.Status = EventStatus.APPROVED;
                return await _eventRepository.UpdateEventAsync(e);
            }
            else if (action == "reject")
            {
                e.Status = EventStatus.REJECTED;
                return await _eventRepository.UpdateEventAsync(e);
            }
            else
            {
                throw new ArgumentException("Invalid action specified.");
            }
        }
    }
}
