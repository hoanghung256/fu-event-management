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
    internal class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student");

            builder.HasIndex(e => e.StudentId, "UQ__Student__4D11D63D2BEA2420").IsUnique();

            builder.HasIndex(e => e.Email, "UQ__Student__AB6E61647AEB2407").IsUnique();

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.AvatarPath)
                .HasDefaultValue("/assets/img/user/default-avatar.jpg")
                .HasColumnName("avatarPath");
            builder.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            builder.Property(e => e.Fullname)
                .HasMaxLength(50)
                .HasColumnName("fullname");
            builder.Property(e => e.Gender)
                .HasConversion<string>()
                .HasMaxLength(6)
                .HasColumnName("gender");
            builder.Property(e => e.Password)
                .HasMaxLength(64)
                .HasColumnName("password");
            builder.Property(e => e.StudentId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("studentId");

            builder.HasMany(d => d.Organizers).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "Follow",
                    r => r.HasOne<Organizer>().WithMany()
                        .HasForeignKey("OrganizerId")
                        .HasConstraintName("FK_Follow_Organizer"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_Follow_Student"),
                    j =>
                    {
                        j.HasKey("StudentId", "OrganizerId");
                        j.ToTable("Follow", tb =>
                        {
                            tb.HasTrigger("trg_DecreaseFollowerCount");
                            tb.HasTrigger("trg_IncreaseFollowerCount");
                        });
                        j.IndexerProperty<int>("StudentId").HasColumnName("studentId");
                        j.IndexerProperty<int>("OrganizerId").HasColumnName("organizerId");
                    });
        }
    }
}
