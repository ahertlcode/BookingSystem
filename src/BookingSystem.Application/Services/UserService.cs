using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.IRepositories;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace BookingSystem.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _hasher;
        private readonly IConfiguration _configuration;
        private SignInManager<ApplicationUser> _signManager;
       
        public UserService(IUserRepository userRepository, IPasswordHasher hasher, IConfiguration configuration, SignInManager<ApplicationUser> signManager)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _configuration = configuration;
            _signManager = signManager;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUserAsync();
        }

        private string GenerateJSONWEBToken(ApplicationUser member)
        {
            var jwtValue = _configuration["Jwt:Key"];
            var jwtSecret = Encoding.UTF8.GetBytes(jwtValue!);
            var securityKey = new SymmetricSecurityKey(jwtSecret);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var fullName = member.FullName;
            string role = member.Role ?? "User";


            var claims = new[]
            {
                // ✅ Standard identity claims
                new Claim(JwtRegisteredClaimNames.Sub, member!.Email!),               
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Email, member!.Email!),

                // ✅ Custom claims               
                new Claim("FullName", fullName),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) throw new UnauthorizedAccessException();
            var verified = _signManager.CheckPasswordSignInAsync(user, password, false).GetAwaiter().GetResult();
            if (!verified.Succeeded) throw new Exception("Invalid credentials");
            // For simplicity, returning a dummy token. In real scenarios, generate JWT or similar tokens.
           
            return GenerateJSONWEBToken(user);
        }

        public async Task<Role> CreateRoleAsync(string name)
        {
            var role = new Role { Name = name };
            await _userRepository.CreateRole(role);
            return role;
        }

        public async Task<User> RegisterUserAsync(string email, string phone_number, string password, string fullName, string role)
        {
            var exists = await _userRepository.GetByEmailAsync(email);
            if (exists != null) throw new Exception("User exists");

            //var hashed = _hasher.HashPassword(password);
            var user = new User { Id = Guid.NewGuid(), Email = email, PhoneNumber = phone_number, Password = password, FullName = fullName, Role = role };
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<Customer> RegisterCustomerAsync(string email, string phone_number, string password, string fullName)
        {
            var exists = await _userRepository.GetByEmailAsync(email);
            if (exists != null) throw new Exception("User exists");
            var hashed = _hasher.HashPassword(password);
            var user = new User { Id = Guid.NewGuid(), Email = email, PhoneNumber = phone_number, Password = hashed, FullName = fullName, Role = "Customer" };
            await _userRepository.AddAsync(user);
            var customer = new Customer { Id = Guid.NewGuid().ToString(), user = user };
            return customer;
        }

        public async Task<Partner> RegisterPartnerAsync(string email, string phone_number, string password, string fullName)
        {
            var exists = await _userRepository.GetByEmailAsync(email);
            if (exists != null) throw new Exception("User exists");
            var hashed = _hasher.HashPassword(password);
            var user = new User { Id = Guid.NewGuid(), Email = email, PhoneNumber = phone_number, Password = hashed, FullName = fullName, Role = "Partner" };
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