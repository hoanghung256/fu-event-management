using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Entities
{
    public class FaceEmbedding
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string EmbeddingJson { get; set; }
        public virtual Student? Student { get; set; }
    }
}
