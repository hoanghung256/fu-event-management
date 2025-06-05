using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class EventImage
{
    public int Id { get; set; }

    public int? EventId { get; set; }

    public string? Path { get; set; }

    public virtual Event? Event { get; set; }
}
