using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class Organizer
{
    public int Id { get; set; }

    public string Acronym { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? AvatarPath { get; set; }

    public string? CoverPath { get; set; }

    public int? FollowerCount { get; set; }

    public bool? IsAdmin { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
