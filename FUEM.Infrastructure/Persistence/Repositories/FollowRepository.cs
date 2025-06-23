using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly FUEMDbContext _context;

        public FollowRepository(FUEMDbContext context)
        {
            _context = context;
        }
    }
}
