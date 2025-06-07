using FUEM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Configurations
{
    internal class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            builder.Property(e => e.LocationDescription)
                .HasMaxLength(1000)
                .HasColumnName("locationDescription");
            builder.Property(e => e.LocationName)
                .HasMaxLength(100)
                .HasColumnName("locationName");
        }
    }
}
