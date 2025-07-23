using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Domain.Common;

namespace FUEM.Application.UseCases.EventUseCases
{
    public class GetAttendedEvents : IGetAttendedEvents
    {
        private readonly IEventRepository _eventRepository;
        public GetAttendedEvents(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Page<Event>> GetAttendedEventsForUserAsync(string userId, int pageNumber, int pageSize)
        {
            return await _eventRepository.GetAttendedEventsForUserIdAsync(userId, pageNumber, pageSize);
        }
    }
}
