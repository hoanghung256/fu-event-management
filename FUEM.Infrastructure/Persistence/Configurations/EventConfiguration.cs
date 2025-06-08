using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Configurations
{
    internal class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Event");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.AvatarPath).HasColumnName("avatarPath");
            builder.Property(e => e.CategoryId).HasColumnName("categoryId");
            builder.Property(e => e.CollaboratorRegisterCount)
                .HasDefaultValue(0)
                .HasColumnName("collaboratorRegisterCount");
            builder.Property(e => e.CollaboratorRegisterDeadline).HasColumnName("collaboratorRegisterDeadline");
            builder.Property(e => e.CollaboratorRegisterLimit).HasColumnName("collaboratorRegisterLimit");
            builder.Property(e => e.DateOfEvent).HasColumnName("dateOfEvent");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.EndTime).HasColumnName("endTime");
            builder.Property(e => e.Fullname)
                .HasMaxLength(200)
                .HasColumnName("fullname");
            builder.Property(e => e.GuestAttendedCount)
                .HasDefaultValue(0)
                .HasColumnName("guestAttendedCount");
            builder.Property(e => e.GuestRegisterCancelCount)
                .HasDefaultValue(0)
                .HasColumnName("guestRegisterCancelCount");
            builder.Property(e => e.GuestRegisterCount)
                .HasDefaultValue(0)
                .HasColumnName("guestRegisterCount");
            builder.Property(e => e.GuestRegisterDeadline).HasColumnName("guestRegisterDeadline");
            builder.Property(e => e.GuestRegisterLimit).HasColumnName("guestRegisterLimit");
            builder.Property(e => e.LocationId).HasColumnName("locationId");
            builder.Property(e => e.OrganizerId).HasColumnName("organizerId");
            builder.Property(e => e.StartTime).HasColumnName("startTime");
            builder.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(15)
                .HasDefaultValue(EventStatus.PENDING)
                .HasColumnName("status");

            builder.HasOne(d => d.Category).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Event_Category");

            builder.HasOne(d => d.Location).WithMany(p => p.Events)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Event_Location");

            builder.HasOne(d => d.Organizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Event_Organizer");
        }
    }
}
