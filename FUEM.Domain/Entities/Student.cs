using FUEM.Domain.Enums;
using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class Student
{
    public int Id { get; set; }

    public string? Fullname { get; set; }

    public string? StudentId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Gender Gender { get; set; }

    public string? AvatarPath { get; set; }

    public virtual ICollection<EventCollaborator> EventCollaborators { get; set; } = new List<EventCollaborator>();

    public virtual ICollection<EventGuest> EventGuests { get; set; } = new List<EventGuest>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Organizer> Organizers { get; set; } = new List<Organizer>();

    public virtual ICollection<FaceEmbedding> FaceEmbeddings { get; set; } = new List<FaceEmbedding>();
}
