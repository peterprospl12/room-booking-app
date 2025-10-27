using Lab2.Extensions;
using Lab2.Helpers;
using Lab2.Models.DTOs;
using Lab2.Services;
using Lab2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Lab2.Controllers
{
    [Authorize]
    public class BookingController(IBookingService bookingService, IRoomService roomService) : Controller
    {
        [HttpGet]
        public IActionResult Calendar(DateTime? date, string viewMode = "Week")
        {
            var selectedDate = date ?? DateTime.Today;
            var mode = Enum.TryParse<CalendarViewMode>(viewMode, out var parsedMode)
                ? parsedMode
                : CalendarViewMode.Week;

            var viewModel = new CalendarViewModel
            {
                SelectedDate = selectedDate,
                ViewMode = mode
            };

            var roomsResult = roomService.GetAllRooms();

            viewModel.Rooms = roomsResult.IsSuccess ? roomsResult.Value : new List<RoomDto>();
            viewModel.Bookings = (List<BookingDto>)[];

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult MyBookings()
        {
            var userId = User.GetUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = bookingService.GetUserBookings(userId.Value);

            if (result.IsFailed)
            {
                TempData["Error"] = "Failed to retrieve bookings";
                return View(new List<BookingDto>());
            }

            return View(result.Value);
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var userId = User.GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var booking = bookingService.GetBookingsForPeriod(DateTime.MinValue, DateTime.MaxValue)
                .Value?.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                TempData["Error"] = "Booking has not been found";
                return RedirectToAction("MyBookings");
            }

            if (booking.UserId != userId.Value && !User.IsInRole("Admin"))
            {
                TempData["Error"] = "You do not have permission to cancel this booking";
                return RedirectToAction("MyBookings");
            }

            var result = bookingService.DeleteBooking(id);

            if (result.IsFailed)
            {
                TempData["Error"] = result.Errors.FirstOrDefault()?.Message ?? "Failed to cancel booking";
            }
            else
            {
                TempData["Success"] = "Booking has been canceled";
            }

            return RedirectToAction("MyBookings");
        }

        [HttpGet]
        public IActionResult ExportMyBookings()
        {
            var userId = User.GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = bookingService.GetUserBookings(userId.Value);

            if (result.IsFailed)
            {
                TempData["Error"] = "Failed to retrieve bookings for export";
                return RedirectToAction("MyBookings");
            }

            var bookings = result.Value.ToList();

            if (!bookings.Any())
            {
                TempData["Error"] = "No bookings to export";
                return RedirectToAction("MyBookings");
            }

            var userName = User.Identity?.Name ?? "User";
            var calendarContent = CalendarHelper.GenerateICalendar(bookings, $"Bookings - {userName}");

            var fileName = $"my_bookings_{DateTime.Now:yyyyMMdd_HHmmss}.ics";

            var bytes = Encoding.UTF8.GetBytes(calendarContent);
            return File(bytes, "text/calendar", fileName);
        }

        [HttpDelete]
        public IActionResult DeleteBooking(int id)
        {
            var result = bookingService.DeleteBooking(id);

            if (result.IsFailed)
            {
                return BadRequest(new { error = result.Errors.FirstOrDefault()?.Message });
            }

            return Ok(new { success = true });
        }
    }
}
