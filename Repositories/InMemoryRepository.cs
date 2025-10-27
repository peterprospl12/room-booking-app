using Lab2.Models;
using System.Collections.Concurrent;
using Lab2.Extensions;

namespace Lab2.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly ConcurrentDictionary<int, List<Booking>> _roomBookings = new();
        private readonly ConcurrentDictionary<int, Room> _rooms = new();
        private readonly ConcurrentDictionary<int, User> _users = new();
        private readonly ConcurrentDictionary<int, Booking> _bookings = new();
        private readonly Lock _bookingLock = new();
        private int _nextUserId = 0;
        private int _nextRoomId = 0;
        private int _nextBookingId = 0;

        public InMemoryRepository()
        {
            var admin = new User
            {
                Id = Interlocked.Increment(ref _nextUserId),
                Name = "Admin",
                Email = "admin@admin.com",
                PasswordHash = HashExtensions.ToSha256Hash("admin123"),
                Role = UserRole.Admin
            };
            _users.TryAdd(admin.Id, admin);
        }

        public bool AddRoom(Room room)
        {
            room.Id = Interlocked.Increment(ref _nextRoomId);
            if (!_rooms.TryAdd(room.Id, room))
            {
                return false;
            }

            _roomBookings[room.Id] = [];
            return true;
        }

        public User? GetUserByEmail(string email)
        {
            return _users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public bool AddUser(User user)
        {
            user.Id = Interlocked.Increment(ref _nextUserId);

            return _users.TryAdd(user.Id, user);
        }

        public User? GetUserById(int id)
        {
            _users.TryGetValue(id, out var user);
            return user;
        }

        public Room? GetRoom(int id)
        {
            _rooms.TryGetValue(id, out var room);
            return room;
        }

        public Room? GetRoomByName(string name)
        {
            return _rooms.Values.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _rooms.Values.ToList();
        }

        public bool DeleteRoom(int id)
        {
            var roomRemoved = _rooms.TryRemove(id, out _);

            if (roomRemoved)
            {
                _roomBookings.TryRemove(id, out _);
            }

            return roomRemoved;
        }

        #region Booking


        public bool AddBooking(Booking booking)
        {
            lock (_bookingLock)
            {

                booking.BookingId = Interlocked.Increment(ref _nextBookingId);


                if (!_bookings.TryAdd(booking.BookingId, booking))
                {
                    return false;
                }

                if (!_roomBookings.TryGetValue(booking.RoomId, out var roomBookingsList))
                {
                    roomBookingsList = [];
                    _roomBookings[booking.RoomId] = roomBookingsList;
                }
                roomBookingsList.Add(booking);

                return true;
            }
        }

        public bool DeleteBooking(int id)
        {
            lock (_bookingLock)
            {
                if (!_bookings.TryRemove(id, out var booking))
                {
                    return false;
                }

                if (_roomBookings.TryGetValue(booking.RoomId, out var roomBookingsList))
                {
                    roomBookingsList.RemoveAll(b => b.BookingId == id);
                }

                return true;
            }
        }

        public Booking? GetBooking(int id)
        {
            lock (_bookingLock)
            {
                _bookings.TryGetValue(id, out var booking);
                return booking;
            }
        }

        public IEnumerable<Booking> GetBookingsForPeriod(DateTime start, DateTime end)
        {
            lock (_bookingLock)
            {
                return _bookings.Values
                    .Where(b => b.StartTime < end && b.EndTime > start)
                    .ToList();
            }
        }

        public IEnumerable<Booking> GetBookingsForRoom(int roomId)
        {
            return _roomBookings.TryGetValue(roomId, out var bookings)
                ? bookings.ToList()
                : [];
        }
        #endregion
    }
}
