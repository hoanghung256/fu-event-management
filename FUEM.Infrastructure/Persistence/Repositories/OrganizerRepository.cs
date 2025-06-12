using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class OrganizerRepository : IOrganizerRepository
    {
        private readonly FUEMDbContext _context;

        public OrganizerRepository(FUEMDbContext context)
        {
            _context = context;
        }

        public async Task<Organizer> GetOrganizerByEmailAsync(string email)
            => await _context.Organizers.FirstOrDefaultAsync(s => s.Email == email);

    }
}
