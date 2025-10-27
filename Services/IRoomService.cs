using FluentResults;
using Lab2.Models.DTOs;

namespace Lab2.Services
{
    public interface IRoomService
    {
        Result<RoomDto> AddRoom(CreateRoomDto roomDto);
        Result<IEnumerable<RoomDto>> GetAllRooms();
        Result DeleteRoom(int id);
    }
}
