using FUEM.Domain.Common;
using FUEM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FUEM.Domain.Entities
{
    public partial class Event
    {
        public int Id { get; set; }

        public int? OrganizerId { get; set; }

        [Required(ErrorMessage = "Fullname is required.")]
        [StringLength(255, ErrorMessage = "Fullname must not exceed 255 characters.")]
        public string? Fullname { get; set; }

        public string? AvatarPath { get; set; }

        [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public int? LocationId { get; set; }

        [Required(ErrorMessage = "Date of Event is required.")]
        public DateOnly? DateOfEvent { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        public TimeOnly? StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        public TimeOnly? EndTime { get; set; }

        public EventStatus Status { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Guest Register Limit must be >= 0.")]
        public int? GuestRegisterLimit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Collaborator Register Limit must be >= 0.")]
        public int? CollaboratorRegisterLimit { get; set; }

        public int? GuestAttendedCount { get; set; }

        public int? GuestRegisterCount { get; set; }

        public int? GuestRegisterCancelCount { get; set; }

        public int? CollaboratorRegisterCount { get; set; }

        public DateOnly? GuestRegisterDeadline { get; set; }

        public DateOnly? CollaboratorRegisterDeadline { get; set; }

        public bool IsNeedTicketPayment { get; set; }

        public int? TicketPrice { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<EventCollaborator> EventCollaborators { get; set; } = new List<EventCollaborator>();

        public virtual ICollection<EventGuest> EventGuests { get; set; } = new List<EventGuest>();

        public virtual ICollection<EventImage> EventImages { get; set; } = new List<EventImage>();

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public virtual Location? Location { get; set; }

        public virtual Organizer? Organizer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime.HasValue && EndTime.HasValue && EndTime <= StartTime)
            {
                yield return new ValidationResult(
                    "End Time must be after Start Time.",
                    [nameof(EndTime)]);
            }

            if (DateOfEvent.HasValue && StartTime.HasValue)
            {
                // Combine DateOfEvent and StartTime into DateTime
                var eventStartDateTime = DateOfEvent.Value.ToDateTime(StartTime.Value);

                if (eventStartDateTime.AddDays(7) < DateTime.Now)
                {
                    yield return new ValidationResult(
                        "Start Time cannot be more than 7 days in the past.",
                        [nameof(StartTime)]);
                }
            }

            if (DateOfEvent.HasValue && GuestRegisterDeadline.HasValue && GuestRegisterDeadline > DateOfEvent)
            {
                yield return new ValidationResult(
                    "Guest Register Deadline must be on or before the Date of Event.",
                    [nameof(GuestRegisterDeadline)]);
            }

            if (DateOfEvent.HasValue && CollaboratorRegisterDeadline.HasValue && CollaboratorRegisterDeadline > DateOfEvent)
            {
                yield return new ValidationResult(
                    "Collaborator Register Deadline must be on or before the Date of Event.",
                    [nameof(CollaboratorRegisterDeadline)]);
            }
        }
    }
}
