using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.EventUseCases
{
    public class GetEvent : IGetEvent
    {
        private readonly IEventRepository _repository;

        public GetEvent(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<Event?> GetEventByName(string eventName)
            => await _repository.GetEventByNameAsync(eventName);

        public async Task<Event?> GetEventById(int eventId)
            => await _repository.GetEventByIdAsync(eventId);
    }
}
