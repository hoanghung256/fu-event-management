using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly FUEMDbContext _context;

        public LocationRepository(FUEMDbContext context)
        {
            _context = context;
        }
    }
}
