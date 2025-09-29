using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid userId);
        Task UpdateUserAsync(User user);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken, Guid userId);
    }
}
