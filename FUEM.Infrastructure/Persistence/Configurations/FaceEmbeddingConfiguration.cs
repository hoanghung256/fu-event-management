using FUEM.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Configurations
{
    internal class FaceEmbeddingConfiguration : IEntityTypeConfiguration<FaceEmbedding>
    {
        public void Configure(EntityTypeBuilder<FaceEmbedding> builder)
        {
            builder.ToTable("FaceEmbedding");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.StudentId)
                .HasColumnName("studentId");
            builder.Property(e => e.EmbeddingJson)
                .HasColumnName("embeddingJson");

            builder.HasOne(d => d.Student).WithMany(p => p.FaceEmbeddings)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_FaceEmbeding_Student");
        }
    }
}
