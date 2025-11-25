using BookingSystem.Domain.Entities;

namespace BookingSystem.Domain.IRepositories
{
    public interface IPartnerTypeRepository
    {
        Task<IEnumerable<PartnerType>> GetAllAsync();
        Task<PartnerType> GetByIdAsync(Guid id);
        Task AddAsync(PartnerType partnerType);
        Task UpdateAsync(PartnerType partnerType);
        Task DeleteAsync(Guid id);
    }
}
