using FluentResults;
using Lab2.Extensions;
using Lab2.Models.DTOs;
using Lab2.Services;
using Lab2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    [Authorize]
    public class BookingController(IBookingService bookingService, IRoomService roomService) : Controller
    {
        [HttpGet]
        [AllowAnonymous]
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
            var bookingsResult = bookingService.GetBookingsForPeriod(
                viewModel.PeriodStart,
                viewModel.PeriodEnd
            );

            viewModel.Rooms = roomsResult.IsSuccess ? roomsResult.Value : new List<RoomDto>();
            viewModel.Bookings = new List<BookingDto>(); 

            return View(viewModel);
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