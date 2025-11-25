using Moq;
using BookingSystem.Domain.Entities;
using BookingSystem.Application.Interfaces;
using BookingSystem.Domain.IRepositories;
using BookingSystem.Application.Services;


namespace BookingSystem.UnitTests{


    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMoc = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService(_userRepositoryMoc.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenDataIsValid()
        {
            _userRepositoryMoc.Setup(r => r.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            _passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<string>())).Returns("hashed_password");
            var user = await _userService.RegisterUserAsync("test@example.com", "password", "Test User", "Customer");
            Assert.NotNull(user);
            Assert.Equal("test@example.com", user.Email);
            _userRepositoryMoc.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_WithExistingEmail_ShouldThrow()
        {
            _userRepositoryMoc.Setup(r => r.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() =>
                _userService.RegisterUserAsync("test@example.com", "password", "Test User", "Customer"));
        }
    }
}