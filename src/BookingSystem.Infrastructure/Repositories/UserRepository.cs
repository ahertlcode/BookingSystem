using BookingSystem.Domain.IRepositories;
using BookingSystem.Domain.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using BookingSystem.Infrastructure.Configurations;


namespace BookingSystem.Infrastructure.Repositories
{
    // Implementation of UserRepository would go here
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<User>("Users");
        }

        public async Task AddAsync(User user) =>
            await _collection.InsertOneAsync(user);

        public async Task<List<User>> GetAllUserAsync() => 
            await _collection.Find(_ => true).ToListAsync();

        public async Task<bool> ExistsAsync(string email) =>
            await _collection.Find(u => u.Email == email).AnyAsync();

        public async Task<User> GetByEmailAsync(string email) =>
            await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<User> GetByIdAsync(string id) => 
            await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(User user) =>
            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);

        public async Task DeleteAsync(string id) => 
            await _collection.DeleteOneAsync(u => u.Id == id);
    }

}