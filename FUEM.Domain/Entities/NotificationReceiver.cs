using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class NotificationReceiver
{
    public int NotificationId { get; set; }

    public int ReceiverId { get; set; }

    public bool IsOrganizer { get; set; }

    public virtual Notification Notification { get; set; } = null!;
}
