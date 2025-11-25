
using BookingSystem.Domain.IRepositories;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Services
{
    public class PartnerTypeService
    {
        private readonly IPartnerTypeRepository _partnerTypeRepository;

        public PartnerTypeService(IPartnerTypeRepository partnerTypeRepository)
        {
            _partnerTypeRepository = partnerTypeRepository;
        }

        public async Task<PartnerType> CreatePartnerTypeAsync(string name, string description)
        {
            var partnerType = new PartnerType
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description
            };
            await _partnerTypeRepository.AddAsync(partnerType);
            return partnerType;
        }

        public async Task<PartnerType> UpdatePartnerTypeAsync(Guid id, string name, string description)
        {
            var partnerType = await _partnerTypeRepository.GetByIdAsync(id);
            if (partnerType == null)
            {
                throw new Exception("Partner type not found.");
            }
            partnerType.Name = name;
            partnerType.Description = description;
            partnerType.UpdatedAt = DateTime.Now;
            await _partnerTypeRepository.UpdateAsync(partnerType);
            return partnerType;
        }

        public async Task DeletePartnerTypeAsync(Guid id)
        {
            var partnerType = await _partnerTypeRepository.GetByIdAsync(id);
            if (partnerType == null)
            {
                throw new Exception("Partner type not found.");
            }
            await _partnerTypeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PartnerType>> GetAllPartnerTypesAsync()
        {
            return await _partnerTypeRepository.GetAllAsync();
        }

    }
}
