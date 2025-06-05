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
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.CategoryDescription)
                .HasMaxLength(1000)
                .HasColumnName("categoryDescription");
            builder.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("categoryName");
            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
        }
    }
}