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
        Task<Guid> CreateAsync(User newUser);
        Task<bool> UpdateAsync(Guid id, User updatedUser);
        Task<bool> DeleteAsync(Guid id);
        Task<List<User>> SearchAsync(string? searchValue, string? sortBy, int page = 1, int pageSize = 10);


        Task<IEnumerable<User>> GetUsersPagedAsync(int page, int rpp);
    }
}

