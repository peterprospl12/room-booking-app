namespace Lab2.Models.DTOs
{
    public record CreateRoomDto(string Name, int Capacity);
    public record RoomDto(int Id, string Name, int Capacity);

}
