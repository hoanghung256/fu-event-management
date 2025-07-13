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
    public class GetEventForGuest : IGetEventForGuest
    {
        private readonly IEventRepository _repository;

        public GetEventForGuest(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<Page<Event>> GetEventForGuestAsync(int page = 1, int pageSize = 10)
            => await _repository.GetEventForGuestAsync(page, pageSize);

        public async Task<Page<Event>> SearchEventAsync(SearchEventCriteria criteria, int page = 1, int pageSize = 10)
            => await _repository.SearchEventAsync(criteria, page, pageSize);
    }
}
