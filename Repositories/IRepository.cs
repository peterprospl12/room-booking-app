using Lab2.Models;

namespace Lab2.Repositories
{
    public interface IRepository
    {
        bool AddRoom(Room room);
        Room? GetRoom(int id);
        Room? GetRoomByName(string name);
        IEnumerable<Room> GetAllRooms();
        bool DeleteRoom(int id);

        bool AddBooking(Booking booking);
        Booking? GetBooking(int id);
        IEnumerable<Booking> GetBookingsForPeriod(DateTime start, DateTime end);
        bool DeleteBooking(int id);

        User? GetUserByEmail(string email);
        bool AddUser(User user);
        User? GetUserById(int id);
        IEnumerable<Booking> GetBookingsForRoom(int roomId);
    }
}
