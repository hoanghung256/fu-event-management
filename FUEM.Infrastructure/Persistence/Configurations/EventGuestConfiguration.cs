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
    internal class EventGuestConfiguration : IEntityTypeConfiguration<EventGuest>
    {
        public void Configure(EntityTypeBuilder<EventGuest> builder)
        {
            builder.HasKey(e => new { e.GuestId, e.EventId });

            builder.ToTable("EventGuest", tb => tb.HasTrigger("trg_updateGuestCounts"));

            builder.Property(e => e.GuestId).HasColumnName("guestId");
            builder.Property(e => e.EventId).HasColumnName("eventId");
            builder.Property(e => e.IsAttended)
                .HasDefaultValue(false)
                .HasColumnName("isAttended");
            builder.Property(e => e.IsCancelRegister)
                .HasDefaultValue(false)
                .HasColumnName("isCancelRegister");
            builder.Property(e => e.IsRegistered)
                .HasDefaultValue(false)
                .HasColumnName("isRegistered");

            builder.HasOne(d => d.Event).WithMany(p => p.EventGuests)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_EventGuest_Event");

            builder.HasOne(d => d.Guest).WithMany(p => p.EventGuests)
                .HasForeignKey(d => d.GuestId)
                .HasConstraintName("FK_EventGuest_Student");
        }
    }
}
