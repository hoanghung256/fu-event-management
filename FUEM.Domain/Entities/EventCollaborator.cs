using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class EventCollaborator
{
    public int StudentId { get; set; }

    public int EventId { get; set; }

    public int? IsCancel { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
