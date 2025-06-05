using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class EventGuest
{
    public int GuestId { get; set; }

    public int EventId { get; set; }

    public bool? IsRegistered { get; set; }

    public bool? IsAttended { get; set; }

    public bool? IsCancelRegister { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Student Guest { get; set; } = null!;
}
