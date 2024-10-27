using InterviewManagement.Domain.Entities;

namespace InterviewManagement.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameOrEmailAsync(string username, string email);
        Task RegisterUserAsync(User user);
        Task<User?> GetUserByCredentialsAsync(string identifier);
        Task AddUserAsync(User user);
        Task<bool> IsUsernameOrEmailTakenAsync(string username, string email);
    }
}
