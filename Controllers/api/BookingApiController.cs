using Lab2.Extensions;
using Lab2.Models.DTOs;
using Lab2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers.Api
{
    [ApiController]
    [Route("api/booking")]
    public class BookingApiController(IBookingService bookingService) : ControllerBase
    {
        [HttpGet("getforday")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetForDay([FromQuery] DateTime? date = null)
        {
            var targetDate = date ?? DateTime.Today;

            var result = bookingService.GetBookingsForDay(targetDate);

            if (result.IsFailed)
            {
                return BadRequest(new
                {
                    success = false,
                    error = result.Errors.FirstOrDefault()?.Message ?? "Failed to retrieve bookings"
                });
            }

            return Ok(new
            {
                success = true,
                date = targetDate.ToString("yyyy-MM-dd"),
                count = result.Value.Count(),
                bookings = result.Value.Select(b => new
                {
                    bookingId = b.BookingId,
                    roomId = b.RoomId,
                    roomName = b.RoomName,
                    userId = b.UserId,
                    userName = b.UserName,
                    startTime = b.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    endTime = b.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    duration = (int)(b.EndTime - b.StartTime).TotalMinutes,
                    isActive = b.StartTime <= DateTime.Now && b.EndTime > DateTime.Now,
                    isPast = b.EndTime < DateTime.Now,
                    isFuture = b.StartTime > DateTime.Now
                })
            });
        }

        [HttpGet("getforroom")]
        [AllowAnonymous]
        public IActionResult GetForRoom([FromQuery] int roomId, [FromQuery] DateTime? date = null)
        {
            var targetDate = date ?? DateTime.Today;
            var result = bookingService.GetBookingsForDay(targetDate);

            if (result.IsFailed)
            {
                return BadRequest(new
                {
                    success = false,
                    error = result.Errors.FirstOrDefault()?.Message
                });
            }

            var roomBookings = result.Value.Where(b => b.RoomId == roomId);

            return Ok(new
            {
                success = true,
                roomId,
                date = targetDate.ToString("yyyy-MM-dd"),
                count = roomBookings.Count(),
                bookings = roomBookings
            });
        }

        [HttpPost("create")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Create([FromBody] CreateBookingDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    success = false,
                    error = "Validation failed",
                    errors = errors
                });
            }

            var userId = User.GetUserId();

            if (userId == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    error = "User not authenticated"
                });
            }

            var result = bookingService.CreateBooking(dto, userId.Value);

            if (result.IsFailed)
            {
                return BadRequest(new
                {
                    success = false,
                    error = result.Errors.FirstOrDefault()?.Message ?? "Failed to create booking"
                });
            }

            var booking = result.Value;
            return Ok(new
            {
                success = true,
                message = "Booking created successfully",
                booking = new
                {
                    bookingId = booking.BookingId,
                    roomId = booking.RoomId,
                    roomName = booking.RoomName,
                    userId = booking.UserId,
                    userName = booking.UserName,
                    startTime = booking.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    endTime = booking.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    duration = (int)(booking.EndTime - booking.StartTime).TotalMinutes
                }
            });
        }
    }

}