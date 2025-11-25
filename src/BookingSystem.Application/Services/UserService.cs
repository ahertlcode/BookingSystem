using BookingSystem.Domain.IRepositories;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;

namespace BookingSystem.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;

        public UserService(IUserRepository userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUserAsync();
        }

        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new Exception("User not found");
            var verified = _hasher.Verify(password, user.Password);
            if (!verified) throw new Exception("Invalid credentials");
            // For simplicity, returning a dummy token. In real scenarios, generate JWT or similar tokens.
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public async Task<User> RegisterUserAsync(string email, string phone_number, string password, string fullName, string role)
        {
            var exists = await _userRepository.GetByEmailAsync(email);
            if (exists != null) throw new Exception("User exists");

            var hashed = _hasher.HashPassword(password);
            var user = new User { Id = Guid.NewGuid().ToString(), Email = email, PhoneNumber = phone_number, Password = hashed, FullName = fullName, Role = role };
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<Customer> RegisterCustomerAsync(string email, string phone_number, string password, string fullName)
        {
            var exists = await _userRepository.GetByEmailAsync(email);
            if (exists != null) throw new Exception("User exists");
            var hashed = _hasher.HashPassword(password);
            var user = new User { Id = Guid.NewGuid().ToString(), Email = email, PhoneNumber = phone_number, Password = hashed, FullName = fullName, Role = "Customer" };
            await _userRepository.AddAsync(user);
            var customer = new Customer { Id = Guid.NewGuid().ToString(), user = user };
            return customer;
        }

        public async Task<Partner> RegisterPartnerAsync(string email, string phone_number, string password, string fullName)
        {
            var exists = await _userRepository.GetByEmailAsync(email);
            if (exists != null) throw new Exception("User exists");
            var hashed = _hasher.HashPassword(password);
            var user = new User { Id = Guid.NewGuid().ToString(), Email = email, PhoneNumber = phone_number, Password = hashed, FullName = fullName, Role = "Partner" };
            await _userRepository.AddAsync(user);
            var partner = new Partner { Id = Guid.NewGuid().ToString(), user = user };
            return partner;
        }

        public async Task<User> UpdateUserAsync(string id, string email, string phone_number, string password, string fullName, string role)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            user.Email = email;
            user.PhoneNumber = phone_number;
            user.Password = _hasher.HashPassword(password);
            user.FullName = fullName;
            user.Role = role;
            user.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            await _userRepository.DeleteAsync(id);
        }
    }
}