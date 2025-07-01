using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class NoticationReceiverRepository : INotificationRepository
    {
        private readonly FUEMDbContext _context;

        public NoticationReceiverRepository(FUEMDbContext context)
        {
            _context = context;
        }
    }
}
