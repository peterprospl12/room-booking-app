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
    }
}