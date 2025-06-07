using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.Event
{
    public class GetEventForGuest : IGetEventForGuest
    {
        private readonly IEventRepository _repository;

        public GetEventForGuest(IEventRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Domain.Entities.Event>> GetEventForGuestAsync()
        {
            return await _repository.GetEventForGuestAsync();
        }
    }
}
