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
    internal class OrganizerConfiguration : IEntityTypeConfiguration<Organizer>
    {
        public void Configure(EntityTypeBuilder<Organizer> builder)
        {
            builder.ToTable("Organizer");

            builder.HasIndex(e => e.Acronym, "UQ__Organize__8172A5316E1AFBED").IsUnique();

            builder.HasIndex(e => e.Email, "UQ__Organize__AB6E61642622C1CA").IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Acronym)
                .HasMaxLength(20)
                .HasColumnName("acronym");
            builder.Property(e => e.AvatarPath)
                .HasDefaultValue("/assets/img/user/default-avatar.jpg")
                .HasColumnName("avatarPath");
            builder.Property(e => e.CategoryId).HasColumnName("categoryId");
            builder.Property(e => e.CoverPath)
                .HasDefaultValue("/assets/img/user/default-banner.png")
                .HasColumnName("coverPath");
            builder.Property(e => e.Description)
                .HasMaxLength(2000)
                .HasColumnName("description");
            builder.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            builder.Property(e => e.FollowerCount)
                .HasDefaultValue(0)
                .HasColumnName("followerCount");
            builder.Property(e => e.Fullname)
                .HasMaxLength(200)
                .HasColumnName("fullname");
            builder.Property(e => e.IsAdmin)
                .HasDefaultValue(false)
                .HasColumnName("isAdmin");
            builder.Property(e => e.Password)
                .HasMaxLength(64)
                .HasColumnName("password");

            builder.HasOne(d => d.Category).WithMany(p => p.Organizers)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Organizer_Category");
        }
    }
}
