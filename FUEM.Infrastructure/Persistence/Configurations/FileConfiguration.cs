using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Configurations
{
    internal class FileConfiguration : IEntityTypeConfiguration<Domain.Entities.File>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.File> builder)
        {
            builder.ToTable("File");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.DisplayName).HasColumnName("displayName");
            builder.Property(e => e.FileType)
                .HasConversion<string>()
                .HasMaxLength(30)
                .HasColumnName("fileType");
            builder.Property(e => e.Path).HasColumnName("path");
            builder.Property(e => e.ProcessNote)
                .HasMaxLength(500)
                .HasColumnName("processNote");
            builder.Property(e => e.ProcessTime)
                .HasColumnType("datetime")
                .HasColumnName("processTime");
            builder.Property(e => e.SendTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("sendTime");
            builder.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("PENDING")
                .HasColumnName("status");
            builder.Property(e => e.SubmitterId).HasColumnName("submitterId");

            builder.HasOne(d => d.Submitter).WithMany(p => p.Files)
                .HasForeignKey(d => d.SubmitterId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_File_Organizer");
        }
    }
}
