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
    /// <summary>
    /// Contains unit tests for the <see cref="UserService"/> class.
    /// </summary>
    public class UserServiceTests
    {
        /// <summary>
        /// Tests that <see cref="UserService.GetUserByIdAsync(Guid)"/> returns a user
        /// when the user with the specified ID exists.
        /// </summary>
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

            // Act
            var result = await userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Test User", result.Name);
            Assert.Equal("test@example.com", result.Email);
        }
    }
}
