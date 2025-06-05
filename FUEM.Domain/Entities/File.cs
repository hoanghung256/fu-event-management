using FUEM.Domain.Enums;
using System;
using System.Collections.Generic;

namespace FUEM.Domain.Entities;

public partial class File
{
    public int Id { get; set; }

    public int? SubmitterId { get; set; }

    public FileType FileType { get; set; }

    public string? DisplayName { get; set; }

    public string? Path { get; set; }

    public DateTime? SendTime { get; set; }

    public FileStatus Status { get; set; }

    public string? ProcessNote { get; set; }

    public DateTime? ProcessTime { get; set; }

    public virtual Organizer? Submitter { get; set; }
}
