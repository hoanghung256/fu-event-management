using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FUEMDbContext _context;

        public FileRepository(FUEMDbContext context)
        {
            _context = context;
        }
    }
}
