using BookingSystem.Domain.Entities;
using BookingSystem.Domain.IRepositories;
using BookingSystem.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookingSystem.Infrastructure.Repositories
{
    public class PartnerTypeRepository : IPartnerTypeRepository
    {
        private readonly IMongoCollection<PartnerType> _collection;

        public PartnerTypeRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<PartnerType>("PartnerTypes");
        }

        public async Task AddAsync(PartnerType partnerType) =>
            await _collection.InsertOneAsync(partnerType);

        public async Task<IEnumerable<PartnerType>> GetAllAsync() => 
            await _collection.Find(_ => true).ToListAsync();

        public async Task<PartnerType> GetByIdAsync(Guid id) => 
            await _collection.Find(pt => pt.Id == id.ToString()).FirstOrDefaultAsync();

        public async Task UpdateAsync(PartnerType partnerType) => 
            await _collection.ReplaceOneAsync(pt => pt.Id == partnerType.Id, partnerType);

        public async Task DeleteAsync(Guid id) => 
            await _collection.DeleteOneAsync(pt => pt.Id == id.ToString());

    }
}
