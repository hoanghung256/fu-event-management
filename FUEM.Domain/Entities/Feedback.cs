using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class Feedback
{
    public int GuestId { get; set; }

    public int EventId { get; set; }

    public string? Content { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Student Guest { get; set; } = null!;
}
