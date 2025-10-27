using FluentResults;
using Lab2.Models;
using Lab2.Models.DTOs;
using Lab2.Repositories;
using Lab2.Extensions;

namespace Lab2.Services
{
    public class UserService(IRepository repository) : IUserService
    {
        public Result<UserDto> RegisterUser(RegisterUserDto userDto)
        {
            if (repository.GetUserByEmail(userDto.Email) != null)
            {
                return Result.Fail("User with this email already exists.");
            }

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = HashExtensions.ToSha256Hash(userDto.Password),
                Role = UserRole.User
            };

            try
            {
                var createdUser = repository.AddUser(user);
                if (!createdUser)
                {
                    Result.Fail<UserDto>("Failed to create user in repository.");
                }

                var resultDto = new UserDto(user.Id, user.Name, user.Email, user.Role);
                return Result.Ok(resultDto);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Repository error: {ex.Message}");
            }
        }

        public Result<UserDto> ValidateUser(LoginUserDto userDto)
        {
            var user = GetUserByEmail(userDto.Email);
            if (user == null)
            {
                return Result.Fail("User with given email does not exist");
            }

            if (!HashExtensions.VerifyHashes(userDto.Password, user.PasswordHash))
            {
                return Result.Fail("Password does not match");
            }

            var resultDto = new UserDto(user.Id, user.Name, user.Email, user.Role);

            return Result.Ok(resultDto);
        }

        private User? GetUserByEmail(string email)
        {
            return repository.GetUserByEmail(email);
        }


    }
}
