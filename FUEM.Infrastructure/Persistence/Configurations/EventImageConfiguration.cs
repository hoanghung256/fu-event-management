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
    internal class EventImageConfiguration : IEntityTypeConfiguration<EventImage>
    {
        public void Configure(EntityTypeBuilder<EventImage> builder)
        {
            builder.ToTable("EventImage");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.EventId).HasColumnName("eventId");
            builder.Property(e => e.Path).HasColumnName("path");

            builder.HasOne(d => d.Event).WithMany(p => p.EventImages)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EventImage_Event");
        }
    }
}
