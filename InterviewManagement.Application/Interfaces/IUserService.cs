using InterviewManagement.Application.DTOs;

namespace InterviewManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<string?> RegisterUserAsync(RegisterUserDto userDto);
        Task<string?> LoginAsync(LoginDto loginDto);

    }
}
