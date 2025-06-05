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
    internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Content)
                .HasMaxLength(300)
                .HasColumnName("content");
            builder.Property(e => e.SenderId).HasColumnName("senderId");
            builder.Property(e => e.SendingTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("sendingTime");
            builder.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            builder.HasOne(d => d.Sender).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Notification_Organizer");
        }
    }
}
