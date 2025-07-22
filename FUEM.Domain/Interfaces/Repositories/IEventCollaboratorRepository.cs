using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface IEventCollaboratorRepository
    {
        Task<bool> IsAlreadyRegisteredAsync(int eventId, int studentId);
        Task AddAsync(EventCollaborator collab);
    }
}
