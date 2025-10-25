using FluentResults;
using Lab2.Models;
using Lab2.Models.DTOs;
using Lab2.Repositories;

namespace Lab2.Services
{
    public class RoomService(IRepository repository) : IRoomService
    {
        public Result<RoomDto> AddRoom(CreateRoomDto roomDto)
        {
            if (repository.GetRoomByName(roomDto.Name) != null)
            {
                return Result.Fail("Room with such name already exists");
            }

            var room = new Room
            {
                Name = roomDto.Name,
                Capacity = roomDto.Capacity
            };

            try
            {
                var success = repository.AddRoom(room);

                if (!success)
                {
                    return Result.Fail<RoomDto>("Failed to create room in repository.");
                }

                var resultDto = new RoomDto(room.Id, room.Name, room.Capacity);

                return Result.Ok(resultDto);
            }
            catch (Exception ex)
            {
                return Result.Fail<RoomDto>($"Repository error: {ex.Message}");
            }
        }

        public Result<IEnumerable<RoomDto>> GetAllRooms()
        {
            try
            {
                var rooms = repository.GetAllRooms();

                var roomDtos = rooms.Select(r => new RoomDto
                (
                    r.Id,
                    r.Name,
                    r.Capacity
                )).ToList();

                return Result.Ok<IEnumerable<RoomDto>>(roomDtos);
            }
            catch (Exception ex)
            {
                return Result.Fail<IEnumerable<RoomDto>>($"Repository error: {ex.Message}");
            }
        }

        public Result DeleteRoom(int id)
        {
            try
            {
                var room = repository.GetRoom(id);
                if (room == null)
                {
                    return Result.Fail("Room not found");
                }

                if (!repository.DeleteRoom(id))
                {
                    return Result.Fail("Failed to remove room from repository");
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
