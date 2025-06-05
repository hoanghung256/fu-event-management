using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryDescription { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Organizer> Organizers { get; set; } = new List<Organizer>();
}
