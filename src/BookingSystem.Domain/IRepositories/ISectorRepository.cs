using BookingSystem.Domain.Entities;


namespace BookingSystem.Domain.IRepositories
{
    public interface ISectorRepository
    {
        Task<IEnumerable<Sector>> GetAllAsync();
        Task<Sector> GetByIdAsync(Guid id);
        Task AddAsync(Sector sector);
        Task UpdateAsync(Sector sector);
        Task DeleteAsync(Guid id);
    }
}
