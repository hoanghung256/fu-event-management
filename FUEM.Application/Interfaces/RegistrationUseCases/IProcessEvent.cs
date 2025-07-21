using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.RegistrationUseCases
{
    public interface IProcessEvent
    {
        public Task<bool> ProcessEventAsync(int eventId, string action);
    }
}
