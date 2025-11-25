using BookingSystem.Domain.Entities;

namespace BookingSystem.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUserAsync();
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(string id);
        Task UpdateAsync(User user);
        Task AddAsync(User user);
        Task<bool> ExistsAsync(string email);
        Task DeleteAsync(string id);
    }
}