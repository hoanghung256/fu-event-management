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
    internal class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(e => new { e.GuestId, e.EventId });

            builder.ToTable("Feedback");

            builder.Property(e => e.GuestId).HasColumnName("guestId");
            builder.Property(e => e.EventId).HasColumnName("eventId");
            builder.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");

            builder.HasOne(d => d.Event).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_Feedback_Event");

            builder.HasOne(d => d.Guest).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.GuestId)
                .HasConstraintName("FK_Feedback_Student");
        }
    }
}
