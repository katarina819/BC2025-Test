using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using BootcampApp.Service;
using BootcampApp.Repository;
using BootcampApp.Model;
using Microsoft.Extensions.Logging;

namespace BootcampApp.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(new User
                {
                    Id = userId,
                    Name = "Test User",
                    Email = "test@example.com"
                });

            var mockLogger = Mock.Of<ILogger<UserService>>();

            var userService = new UserService(mockUserRepository.Object, mockLogger);

            
            var result = await userService.GetUserByIdAsync(userId);

            
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
        }
    }
}

