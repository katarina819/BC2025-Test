using System.Buffers;
using System.Text;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Common.DTOs;
using BootcampApp.Common.LoginRequest;
using BootcampApp.Common.Mappers;
using BootcampApp.Model;
using BootcampApp.Repository;
using BootcampApp.Common;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Security.Cryptography;
using BCrypt.Net;

namespace BootcampApp.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public Task<List<User>> GetAllUsersAsync() => _userRepository.GetAllAsync();

        /// <summary>
        /// Authenticates a user by their name and email asynchronously.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="email">The user's email.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<User?> LoginUserAsync(string name, string email)
        {
            return await _userRepository.GetByNameAndEmailAsync(name, email);
        }

        /// <summary>
        /// Retrieves a user by ID and maps it to a UserDto asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A <see cref="UserDto"/> if found; otherwise, null.</returns>
        public async Task<UserDto?> GetUserByIdDtoAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.Id == Guid.Empty)
                return null;

            return UserMapper.ToDto(user);
        }

        /// <summary>
        /// Retrieves a user by ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The <see cref="User"/> if found; otherwise, null.</returns>
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Retrieves a paged list of users asynchronously.
        /// </summary>
        /// <param name="page">The page number to retrieve (default 1).</param>
        /// <param name="rpp">The number of results per page (default 10).</param>
        /// <returns>A list of users for the specified page.</returns>
        public Task<List<User>> GetAllUsersAsync(int page = 1, int rpp = 10)
        {
            _logger.LogInformation("Fetching users - Page: {Page}, Page Size: {PageSize}", page, rpp);
            return _userRepository.GetUsersPagedAsync(page, rpp);
        }

        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>The GUID of the newly created user.</returns>
        public Task<Guid> CreateUserAsync(User user) => _userRepository.CreateAsync(user);

        /// <summary>
        /// Updates user data asynchronously using the provided DTO.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="dto">The data transfer object containing updated user data.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateUserDataAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            UpdateUserFromDto(user, dto);

            return await _userRepository.UpdateAsync(id, user);
        }

        /// <summary>
        /// Deletes a user asynchronously by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public Task<bool> DeleteUserAsync(Guid id) => _userRepository.DeleteAsync(id);

        /// <summary>
        /// Retrieves a paged collection of users asynchronously.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>An enumerable collection of users.</returns>
        public async Task<IEnumerable<User>> GetUsersPagedAsync(int page, int pageSize)
        {
            return await _userRepository.GetUsersPagedAsync(page, pageSize);
        }

        /// <summary>
        /// Counts the number of users that match a filter asynchronously.
        /// </summary>
        /// <param name="searchValue">Optional search string to filter users.</param>
        /// <returns>The number of filtered users.</returns>
        public async Task<int> CountFilteredUsersAsync(string? searchValue)
        {
            return await _userRepository.CountFilteredUsersAsync(searchValue);
        }

        /// <summary>
        /// Counts the total number of users asynchronously.
        /// </summary>
        /// <returns>The total user count.</returns>
        public async Task<int> CountUsersAsync()
        {
            return await _userRepository.CountUsersAsync();
        }

        /// <summary>
        /// Searches users asynchronously with paging.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>A list of users matching the search criteria.</returns>
        public async Task<List<User>> SearchAsync(int page, int pageSize)
        {
            return await _userRepository.SearchAsync(page, pageSize);
        }

        /// <summary>
        /// Updates user properties from the provided DTO.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <param name="dto">The data transfer object containing updated user data.</param>
        private void UpdateUserFromDto(User user, UpdateUserDto dto)
        {
            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Age = dto.Age;

            if (user.Profile == null)
                user.Profile = new UserProfile();

            user.Profile.PhoneNumber = dto.PhoneNumber;
            user.Profile.Address = dto.Address;
        }

        /// <summary>
        /// Updates a user asynchronously and returns the updated entity.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="dto">The data transfer object containing updated user data.</param>
        /// <returns>The updated <see cref="User"/> entity.</returns>
        /// <exception cref="Exception">Thrown if the user is not found or update fails.</exception>
        public async Task<User> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                throw new Exception("User not found");

            UpdateUserFromDto(existingUser, dto);

            var updated = await _userRepository.UpdateAsync(id, existingUser);
            if (!updated)
                throw new Exception("Failed to update user");

            return await _userRepository.GetByIdAsync(id);
        }
    }
}
