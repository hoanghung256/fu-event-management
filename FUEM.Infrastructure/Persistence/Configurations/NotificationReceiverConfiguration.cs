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
    internal class NotificationReceiverConfiguration : IEntityTypeConfiguration<NotificationReceiver>
    {
        public void Configure(EntityTypeBuilder<NotificationReceiver> builder)
        {
            builder.HasKey(e => new { e.NotificationId, e.ReceiverId, e.IsOrganizer });

            builder.ToTable("NotificationReceiver");

            builder.Property(e => e.NotificationId).HasColumnName("notificationId");
            builder.Property(e => e.ReceiverId).HasColumnName("receiverId");
            builder.Property(e => e.IsOrganizer).HasColumnName("isOrganizer");

            builder.HasOne(d => d.Notification).WithMany(p => p.NotificationReceivers)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK_NotificationReceiver_Notification");
        }
    }
}
