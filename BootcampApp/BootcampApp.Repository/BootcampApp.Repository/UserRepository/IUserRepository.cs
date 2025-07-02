using BootcampApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Defines repository methods for managing User entities.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A list of all users.</returns>
        Task<List<User>> GetAllAsync();

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>The unique identifier of the created user.</returns>
        Task<Guid> CreateAsync(User user);

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="updatedUser">The updated user data.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateAsync(Guid id, User updatedUser);

        /// <summary>
        /// Deletes a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Searches users with pagination asynchronously.
        /// </summary>
        /// <param name="page">The page number (starting from 1).</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>A list of users matching the search criteria for the specified page.</returns>
        Task<List<User>> SearchAsync(int page, int pageSize);

        /// <summary>
        /// Retrieves a paged list of users asynchronously.
        /// </summary>
        /// <param name="page">The page number (starting from 1).</param>
        /// <param name="pageSize">The number of users per page.</param>
        /// <returns>A list of users for the specified page.</returns>
        Task<List<User>> GetUsersPagedAsync(int page, int pageSize);

        /// <summary>
        /// Counts the total number of users matching an optional search filter asynchronously.
        /// </summary>
        /// <param name="searchValue">The optional search value to filter users.</param>
        /// <returns>The count of filtered users.</returns>
        Task<int> CountFilteredUsersAsync(string? searchValue);

        /// <summary>
        /// Counts the total number of users asynchronously.
        /// </summary>
        /// <returns>The total count of users.</returns>
        Task<int> CountUsersAsync();

        /// <summary>
        /// Retrieves a user by their name and email asynchronously.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="email">The user's email.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetByNameAndEmailAsync(string name, string email);

        /// <summary>
        /// Retrieves a user along with their profile information by user ID asynchronously.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>The user with profile if found; otherwise, null.</returns>
        Task<User?> GetUserWithProfileByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a user by their username asynchronously.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Retrieves a user by their username asynchronously.
        /// (Alias of GetByUsernameAsync)
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
