using FluentResults;
using Lab2.Models;
using Lab2.Models.DTOs;
using Lab2.ViewModels;

namespace Lab2.Services
{
    public interface IUserService
    {
        Result<UserDto> RegisterUser(RegisterUserDto userDto);
        Result<UserDto> ValidateUser(LoginUserDto userDto);  

    }
}
