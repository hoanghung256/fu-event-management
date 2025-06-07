using FUEM.Domain.Common;
using FUEM.Domain.Enums;
using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class Event
{
    public int Id { get; set; }

    public int? OrganizerId { get; set; }

    public string? Fullname { get; set; }

    public string? AvatarPath { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public int? LocationId { get; set; }

    public DateOnly? DateOfEvent { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public EventStatus Status { get; set; }

    public int? GuestRegisterLimit { get; set; }

    public int? CollaboratorRegisterLimit { get; set; }

    public int? GuestAttendedCount { get; set; }

    public int? GuestRegisterCount { get; set; }

    public int? GuestRegisterCancelCount { get; set; }

    public int? CollaboratorRegisterCount { get; set; }

    public DateOnly? GuestRegisterDeadline { get; set; }

    public DateOnly? CollaboratorRegisterDeadline { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<EventCollaborator> EventCollaborators { get; set; } = new List<EventCollaborator>();

    public virtual ICollection<EventGuest> EventGuests { get; set; } = new List<EventGuest>();

    public virtual ICollection<EventImage> EventImages { get; set; } = new List<EventImage>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Location? Location { get; set; }

    public virtual Organizer? Organizer { get; set; }
}
