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
    public class GetRecentEvents : IGetRecentEvents
    {
        private readonly IEventRepository _repository;

        public GetRecentEvents(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Event>> GetRecentEventsByOrganizerId(int organizerId, int count = 10)
            => await _repository.GetRecentEventsByOrganizerId(organizerId, count);
    }
}
