using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class Location
{
    public int Id { get; set; }

    public string? LocationName { get; set; }

    public string? LocationDescription { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
