using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUEM.Domain.Entities;

public partial class EventImage
{
    public int Id { get; set; }

    public int? EventId { get; set; }

    public string? Path { get; set; }

    [NotMapped]
    public Stream Stream { get; set; }

    public virtual Event? Event { get; set; }
}
