using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Common.DTOs;
using BootcampApp.Model;
using BootcampApp.Common;

public interface IUserService
{
    /// <summary>
    /// Retrieves all users asynchronously.
    /// </summary>
    /// <returns>A list of all <see cref="User"/> objects.</returns>
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Retrieves a user by their ID and returns a UserDto asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The <see cref="UserDto"/> if found; otherwise, null.</returns>
    Task<UserDto?> GetUserByIdDtoAsync(Guid id);

    /// <summary>
    /// Retrieves a user by their ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The <see cref="User"/> if found; otherwise, null.</returns>
    Task<User?> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user entity to create.</param>
    /// <returns>The GUID of the newly created user.</returns>
    Task<Guid> CreateUserAsync(User user);

    /// <summary>
    /// Updates user data asynchronously using a DTO.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="dto">The data transfer object containing updated user data.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateUserDataAsync(Guid id, UpdateUserDto dto);

    /// <summary>
    /// Deletes a user asynchronously by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteUserAsync(Guid id);

    /// <summary>
    /// Counts the number of users matching the filter asynchronously.
    /// </summary>
    /// <param name="searchValue">The search string to filter users (optional).</param>
    /// <returns>The count of filtered users.</returns>
    Task<int> CountFilteredUsersAsync(string? searchValue);

    /// <summary>
    /// Retrieves a paged list of users asynchronously.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of users per page.</param>
    /// <returns>An enumerable collection of users on the specified page.</returns>
    Task<IEnumerable<User>> GetUsersPagedAsync(int page, int pageSize);

    /// <summary>
    /// Counts the total number of users asynchronously.
    /// </summary>
    /// <returns>The total user count.</returns>
    Task<int> CountUsersAsync();

    /// <summary>
    /// Searches for users asynchronously with paging.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of users per page.</param>
    /// <returns>A list of users matching the search criteria on the specified page.</returns>
    Task<List<User>> SearchAsync(int page, int pageSize);

    /// <summary>
    /// Authenticates a user by their name and email asynchronously.
    /// </summary>
    /// <param name="name">The name of the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <returns>The authenticated <see cref="User"/> if credentials match; otherwise, null.</returns>
    Task<User?> LoginUserAsync(string name, string email);

    /// <summary>
    /// Updates a user asynchronously using a DTO.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="dto">The data transfer object containing updated user data.</param>
    /// <returns>The updated <see cref="User"/> object.</returns>
    Task<User> UpdateUserAsync(Guid id, UpdateUserDto dto);
}
