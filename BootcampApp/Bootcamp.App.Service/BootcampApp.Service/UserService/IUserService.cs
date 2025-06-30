using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Common.DTOs;
using BootcampApp.Model;
using BootcampApp.Common;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();


    Task<UserDto?> GetUserByIdDtoAsync(Guid id);
    Task<User?> GetUserByIdAsync(Guid id);

    Task<Guid> CreateUserAsync(User user);
    Task<bool> UpdateUserDataAsync(Guid id, UpdateUserDto dto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<int> CountFilteredUsersAsync(string? searchValue);
    Task<IEnumerable<User>> GetUsersPagedAsync(int page, int pageSize);
    Task<int> CountUsersAsync();
    Task<List<User>> SearchAsync(int page, int pageSize);
    Task<User?> LoginUserAsync(string name, string email);
    Task<User> UpdateUserAsync(Guid id, UpdateUserDto dto);
    




}





