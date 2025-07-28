using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    internal class FaceEmbeddingRepository : IFaceEmbeddingRepository
    {
        private readonly FUEMDbContext _context;

        public FaceEmbeddingRepository(FUEMDbContext context)
        {
            _context = context;
        }

        public async Task SaveEmbeddingsAsync(List<FaceEmbedding> embeddings)
        {
            await _context.FaceEmbeddings.AddRangeAsync(embeddings);
            await _context.SaveChangesAsync();
        }
    }
}
