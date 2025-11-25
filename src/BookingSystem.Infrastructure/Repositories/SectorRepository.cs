using BookingSystem.Domain.Entities;
using BookingSystem.Domain.IRepositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookingSystem.Infrastructure.Configurations;


namespace BookingSystem.Infrastructure.Repositories
{
    public class SectorRepository : ISectorRepository
    {
        // Implementation of SectorRepository would go here
        private readonly IMongoCollection<Sector> _collection;
        public SectorRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {            
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<Sector>("Sectors");
        }

        public async Task AddAsync(Sector sector) =>
            await _collection.InsertOneAsync(sector);

        public async Task<IEnumerable<Sector>> GetAllAsync() => 
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Sector> GetByIdAsync(Guid id) => 
            await _collection.Find(s => s.Id == id.ToString()).FirstOrDefaultAsync();

        public async Task UpdateAsync(Sector sector) => 
            await _collection.ReplaceOneAsync(s => s.Id == sector.Id, sector);

        public async Task DeleteAsync(Guid id) => 
            await _collection.DeleteOneAsync(s => s.Id == id.ToString());
    }
}
