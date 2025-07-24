using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class EventCollaboratorRepository : IEventCollaboratorRepository
    {
        private readonly FUEMDbContext _context;
        public EventCollaboratorRepository(FUEMDbContext context) => _context = context;

        public async Task<bool> IsAlreadyRegisteredAsync(int eventId, int studentId)
        {
            return await _context.EventCollaborators
                .AnyAsync(x => x.EventId == eventId && x.StudentId == studentId && x.IsCancel != 1);
        }

        public async Task AddAsync(EventCollaborator collab)
        {
            _context.EventCollaborators.Add(collab);
            await _context.SaveChangesAsync();
        }
    }
}
