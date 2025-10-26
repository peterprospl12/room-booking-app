using FluentResults;
using Lab2.Models.DTOs;

namespace Lab2.Services
{
    public interface IBookingService
    {
        Result<IEnumerable<BookingDto>> GetBookingsForPeriod(DateTime viewModelPeriodStart, DateTime viewModelPeriodEnd);
        Result<IEnumerable<BookingDto>> GetBookingsForDay(DateTime date);
        Result<BookingDto> CreateBooking(CreateBookingDto dto, int userId);
        Result DeleteBooking(int id);
    }
}
