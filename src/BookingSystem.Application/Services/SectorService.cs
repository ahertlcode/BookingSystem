using BookingSystem.Domain.Entities;
using BookingSystem.Domain.IRepositories;


namespace BookingSystem.Application.Services
{
    public class SectorService
    {
        private readonly ISectorRepository _sectorRepository;
        public SectorService(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }

        private void ValidateSectorName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Sector name cannot be empty.");
            }
            // Additional validation rules can be added here
        }

        public async Task<Sector> CreateSectorAsync(string name, string description)
        {
            ValidateSectorName(name);
            var sector = new Sector
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description
            };
            await _sectorRepository.AddAsync(sector);
            return sector;
        }

        public async Task<Sector> UpdateSectorAsync(Guid id, string name, string description)
        {
            ValidateSectorName(name);
            var sector = await _sectorRepository.GetByIdAsync(id);
            if (sector == null)
            {
                throw new Exception("Sector not found.");
            }
            sector.Name = name;
            sector.Description = description;
            sector.UpdatedAt = DateTime.Now;
            await _sectorRepository.UpdateAsync(sector);
            return sector;
        }

        public async Task DeleteSectorAsync(Guid id)
        {
            var sector = await _sectorRepository.GetByIdAsync(id);
            if (sector == null)
            {
                throw new Exception("Sector not found.");
            }
            await _sectorRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Sector>> GetAllSectorsAsync()
        {
            return await _sectorRepository.GetAllAsync();
        }
    }
}
