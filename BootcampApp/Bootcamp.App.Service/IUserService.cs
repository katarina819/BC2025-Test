using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Common.DTOs;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    
    Task<List<UserDto>> GetAllUsersDtoAsync(string? searchValue, string? sortBy, int page, int pageSize);
    Task<UserDto?> GetUserByIdDtoAsync(Guid id);
    Task<User?> GetUserByIdAsync(Guid id);

    Task<Guid> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(Guid id, User user);
    Task<bool> DeleteUserAsync(Guid id);
}





