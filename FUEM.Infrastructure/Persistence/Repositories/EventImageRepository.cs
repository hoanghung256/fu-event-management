using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class EventImageRepository : IEventImageRepository
    {
        private readonly FUEMDbContext _context;

        public EventImageRepository(FUEMDbContext context)
        {
            _context = context;
        }

        public async Task AddImagesAsync(List<EventImage> images)
        {
            await _context.EventImages.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }
    }
}
