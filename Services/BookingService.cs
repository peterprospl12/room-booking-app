using FluentResults;
using Lab2.Models;
using Lab2.Models.DTOs;
using Lab2.Repositories;

namespace Lab2.Services
{
    public class BookingService(IRepository repository) : IBookingService
    {

        public Result<IEnumerable<BookingDto>> GetBookingsForPeriod(DateTime start, DateTime end)
        {
            try
            {
                var bookings = repository.GetBookingsForPeriod(start, end);
                var bookingDtos = new List<BookingDto>();
                var orphanedBookingIds = new List<int>();

                foreach (var booking in bookings)
                {
                    var room = repository.GetRoom(booking.RoomId);
                    if (room == null)
                    {
                        orphanedBookingIds.Add(booking.BookingId);
                        continue;
                    }

                    var user = repository.GetUserById(booking.UserId);
                    if (user == null)
                    {
                        orphanedBookingIds.Add(booking.BookingId);
                        continue;
                    }

                    bookingDtos.Add(new BookingDto(
                        booking.BookingId,
                        booking.RoomId,
                        room.Name,
                        booking.UserId,
                        user.Name,
                        booking.StartTime,
                        booking.EndTime
                    ));
                }

                foreach (var orphanedId in orphanedBookingIds)
                {
                    repository.DeleteBooking(orphanedId);
                }

                return Result.Ok<IEnumerable<BookingDto>>(bookingDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<BookingDto>>($"Failed to retrieve bookings: {ex.Message}");
            }
        }

        public Result<IEnumerable<BookingDto>> GetBookingsForDay(DateTime date)
        {
            var dayStart = date.Date;
            var dayEnd = date.Date.AddDays(1);

            return GetBookingsForPeriod(dayStart, dayEnd);
        }

        public Result<IEnumerable<BookingDto>> GetUserBookings(int userId)
        {
            try
            {
                var now = DateTime.Now;
                var bookings = repository.GetBookingsForPeriod(now, now.AddDays(360));

                var userBookings = bookings
                    .Where(b => b.UserId == userId && b.EndTime > now)
                    .OrderBy(b => b.StartTime)
                    .ToList();

                var bookingDtos = new List<BookingDto>();

                foreach (var booking in userBookings)
                {
                    var room = repository.GetRoom(booking.RoomId);
                    if (room == null) continue;

                    var user = repository.GetUserById(booking.UserId);
                    if (user == null) continue;

                    bookingDtos.Add(new BookingDto(
                        booking.BookingId,
                        booking.RoomId,
                        room.Name,
                        booking.UserId,
                        user.Name,
                        booking.StartTime,
                        booking.EndTime
                    ));
                }

                return Result.Ok<IEnumerable<BookingDto>>(bookingDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<BookingDto>>($"Failed to retrieve user bookings: {ex.Message}");
            }
        }

        public Result<BookingDto> CreateBooking(CreateBookingDto dto, int userId)
        {
            var room = repository.GetRoom(dto.RoomId);
            
            if (room == null)
            {
                return Result.Fail<BookingDto>("Room not found");
            }

            var user = repository.GetUserById(userId);
            if (user == null)
            {
                return Result.Fail<BookingDto>("User not found");
            }

            var existingBookings = repository.GetBookingsForRoom(dto.RoomId);
            var hasOverlap = existingBookings.Any(b =>
                dto.StartTime < b.EndTime && dto.EndTime > b.StartTime);

            if (hasOverlap)
            {
                return Result.Fail("This time slot is already booked. Please choose a different time.");
            }

            var booking = new Booking()
            {
                RoomId = dto.RoomId,
                UserId = userId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            if (!repository.AddBooking(booking))
            {
                return Result.Fail("Failed to create booking in repository");
            }

            var bookingDto = new BookingDto(
                booking.BookingId,
                booking.RoomId,
                room.Name,
                booking.UserId,
                user.Name,
                booking.StartTime,
                booking.EndTime
            );

            return Result.Ok(bookingDto);
        }

        public Result DeleteBooking(int id)
        {
            try
            {
                var booking = repository.GetBooking(id);
                if (booking == null)
                {
                    return Result.Fail("Booking not found");
                }

                if (!repository.DeleteBooking(id))
                {
                    return Result.Fail("Failed to remove booking from repository");
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Repository error: {ex.Message}");
            }
        }
    }
}
