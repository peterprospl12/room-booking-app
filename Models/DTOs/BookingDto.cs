using System.ComponentModel.DataAnnotations;

namespace Lab2.Models.DTOs
{
    public record BookingDto(
        int BookingId,
        int RoomId,
        string RoomName,
        int UserId,
        string UserName,
        DateTime StartTime,
        DateTime EndTime
    );

    public record CreateBookingDto
    {
        [Required(ErrorMessage = "Room ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Room ID")]
        public int RoomId { get; init; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; init; }

        [Required(ErrorMessage = "End time is required")]
        public DateTime EndTime { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (StartTime >= EndTime)
            {
                errors.Add(new ValidationResult(
                    "Start time must be before end time",
                    [nameof(StartTime), nameof(EndTime)]
                ));
            }

            if (StartTime < DateTime.Now)
            {
                errors.Add(new ValidationResult(
                    "Cannot create booking in the past",
                    [nameof(StartTime)]
                ));
            }

            var duration = EndTime - StartTime;
            if (duration.TotalMinutes < 15)
            {
                errors.Add(new ValidationResult(
                    "Booking duration must be at least 15 minutes",
                    [nameof(StartTime), nameof(EndTime)]
                ));
            }

            if (duration.TotalHours > 3)
            {
                errors.Add(new ValidationResult(
                    "Booking duration cannot exceed 3 hours",
                    [nameof(StartTime), nameof(EndTime)]
                ));
            }

            return errors;
        }
    }
}