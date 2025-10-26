using FluentResults;
using Lab2.Models.DTOs;

namespace Lab2.Services
{
    public interface IUserService
    {
        Result<UserDto> RegisterUser(RegisterUserDto userDto);
        Result<UserDto> ValidateUser(LoginUserDto userDto);  

    }
}
