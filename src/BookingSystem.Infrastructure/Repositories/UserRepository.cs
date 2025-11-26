using BookingSystem.Domain.IRepositories;
using BookingSystem.Domain.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using BookingSystem.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;



namespace BookingSystem.Infrastructure.Repositories
{
    // Implementation of UserRepository would go here
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        


        public UserRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, 
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<User>("Users");
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateRole(Role userRole)
        {
            var roleExists = await _roleManager.RoleExistsAsync(userRole.Name);
            if (!roleExists)
            {
                var role = new ApplicationRole { Name = userRole.Name };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
        
        public async Task AddAsync(User user)
        {
            var exists = await _userManager.FindByEmailAsync(user.Email);
            if (exists != null)
            {
                throw new Exception("User with this email already exists.");
            }
            var appUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };
            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            
            await _userManager.AddToRoleAsync(appUser, user.Role);
            await _userManager.UpdateAsync(appUser);            
        }

        public async Task<List<User>> GetAllUserAsync() => 
            await _collection.Find(_ => true).ToListAsync();

        public async Task<bool> ExistsAsync(string email) =>
            await _collection.Find(u => u.Email == email).AnyAsync();

        
        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(appUser);
            appUser.Role = roles.FirstOrDefault();


            return appUser;
        }

        public async Task<User> GetByIdAsync(string id) => 
            await _collection.Find(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();

        public async Task UpdateAsync(User user) =>
            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);

        public async Task DeleteAsync(string id) => 
            await _collection.DeleteOneAsync(u => u.Id == Guid.Parse(id));
    }

}