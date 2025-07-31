using System;
using System.Collections.Generic;
using FUEM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUEM.Infrastructure.Persistence;

public partial class FUEMDbContext : DbContext
{
    public FUEMDbContext()
    {
    }

    public FUEMDbContext(DbContextOptions<FUEMDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCollaborator> EventCollaborators { get; set; }

    public virtual DbSet<EventGuest> EventGuests { get; set; }

    public virtual DbSet<EventImage> EventImages { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Domain.Entities.File> Files { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationReceiver> NotificationReceivers { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<FaceEmbedding> FaceEmbeddings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=EventManagement;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FUEMDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
