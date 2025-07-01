using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FUEMDbContext _context;

        public FeedbackRepository(FUEMDbContext context)
        {
            _context = context;
        }
    }
}
