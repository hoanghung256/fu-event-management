using FUEM.Application.Interfaces.EventUseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.EventUseCases
{
    public class CreateEvent : ICreateEvent
    {
        public Task<IEnumerable<Domain.Entities.Event>> CreateEventAsync()
        {
            throw new NotImplementedException();
        }
    }
}
