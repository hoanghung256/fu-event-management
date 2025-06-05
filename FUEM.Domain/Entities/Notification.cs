using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public int? SenderId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? SendingTime { get; set; }

    public virtual ICollection<NotificationReceiver> NotificationReceivers { get; set; } = new List<NotificationReceiver>();

    public virtual Organizer? Sender { get; set; }
}
