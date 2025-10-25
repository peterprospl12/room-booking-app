namespace Lab2.Models.DTOs
{
    public record RegisterUserDto(string Name, string Email, string Password);

    public record LoginUserDto(string Email, string Password);

    public record UserDto(int Id, string Name, string Email, UserRole Role);
}