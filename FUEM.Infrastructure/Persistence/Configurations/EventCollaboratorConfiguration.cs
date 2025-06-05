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
    internal class EventCollaboratorConfiguration : IEntityTypeConfiguration<EventCollaborator>
    {
        public void Configure(EntityTypeBuilder<EventCollaborator> builder)
        {
            builder.HasKey(e => new { e.StudentId, e.EventId });

            builder.ToTable("EventCollaborator", tb =>
            {
                tb.HasTrigger("trg_DecreaseCollaboratorCount");
                tb.HasTrigger("trg_IncreaseCollaboratorCount");
            });

            builder.Property(e => e.StudentId).HasColumnName("studentId");
            builder.Property(e => e.EventId).HasColumnName("eventId");
            builder.Property(e => e.IsCancel)
                .HasDefaultValue(0)
                .HasColumnName("isCancel");

            builder.HasOne(d => d.Event).WithMany(p => p.EventCollaborators)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_EventCollaborator_Event");

            builder.HasOne(d => d.Student).WithMany(p => p.EventCollaborators)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_EventCollaborator_Student");
        }
    }
}
