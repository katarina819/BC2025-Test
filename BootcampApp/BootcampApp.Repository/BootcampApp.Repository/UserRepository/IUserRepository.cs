using BootcampApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BootcampApp.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(User user);
        Task<bool> UpdateAsync(Guid id, User updatedUser);
        Task<bool> DeleteAsync(Guid id);
        Task<List<User>> SearchAsync(int page, int pageSize);


        Task<List<User>> GetUsersPagedAsync(int page, int pageSize);
        Task<int> CountFilteredUsersAsync(string? searchValue);
        Task<int> CountUsersAsync();
        Task<User?> GetByNameAndEmailAsync(string name, string email);
        Task<User?> GetUserWithProfileByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetUserByUsernameAsync(string username);










    }
}

