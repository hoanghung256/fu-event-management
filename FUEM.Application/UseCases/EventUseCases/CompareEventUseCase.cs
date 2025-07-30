using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.EventUseCases
{
    public class CompareEventUseCase : ICompareEventUseCase
    {
        private readonly IEventRepository _eventRepository;

        public CompareEventUseCase(IEventRepository eventRepository) 
        {
            _eventRepository = eventRepository;
        }

        public async Task<List<Event>> CompareEvent(int eventId, int? comparedId)
        {
            var baseEvent = await _eventRepository.GetEventByIdAsync(eventId);

            if (comparedId != null)
            {
                var comparedEvent = await _eventRepository.GetEventByIdAsync((int)comparedId);
                return new List<Event> { baseEvent, comparedEvent };
            }
            return new List<Event> { baseEvent };
        }

        public async Task<List<Event>> GetRemainEventAsync(string organizerId, int eventId, int? comparedId) 
        {
            Page<Event> recentOrganizedEventPage = await _eventRepository.GetOrganizedEventsByOrganizerIdAsync(organizerId, 1, 10);
            var remainEvent = recentOrganizedEventPage.Items
                .Where(e => e.Id != eventId && e.Id != comparedId)
                .ToList();
            return remainEvent;
        }
    }
}
