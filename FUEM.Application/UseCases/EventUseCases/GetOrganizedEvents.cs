using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Domain.Common;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;


namespace FUEM.Application.UseCases.EventUseCases
{
    public class GetOrganizedEvents : IGetOrganizedEvents
    {
        private readonly IEventRepository _eventRepository;

        public GetOrganizedEvents(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Page<Event>> GetOrganizedEventsForOrganizerAsync(string organizerId, int pageNumber, int pageSize)
        {
            return await _eventRepository.GetOrganizedEventsByOrganizerIdAsync(organizerId, pageNumber, pageSize);
        }

        public async Task<Page<Event>> GetOrganizedEventsForAdminAsync(int pageNumber, int pageSize)
        {
            return await _eventRepository.GetAllOrganizedEventsAsync(pageNumber, pageSize);
        }
    }
}