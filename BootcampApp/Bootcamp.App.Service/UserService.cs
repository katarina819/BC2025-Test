using System.Text;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;
using Npgsql;

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

        public Task<List<User>> GetAllUsersAsync() => _userRepository.GetAllAsync();

        public Task<User?> GetUserByIdAsync(Guid id) => _userRepository.GetByIdAsync(id);

        public Task<List<User>> GetAllUsersAsync(string? searchValue, string? sortBy, int page, int pageSize)
        {
            _logger.LogInformation("Dohvaćam korisnike s filterom: {SearchValue}, sort: {SortBy}, stranica: {Page}, veličina: {PageSize}", searchValue, sortBy, page, pageSize);
            return _userRepository.SearchAsync(searchValue, sortBy, page, pageSize);
        }

        public Task<Guid> CreateUserAsync(User user) => _userRepository.CreateAsync(user);

        public Task<bool> UpdateUserAsync(Guid id, User user) => _userRepository.UpdateAsync(id, user);

        public Task<bool> DeleteUserAsync(Guid id) => _userRepository.DeleteAsync(id);

        public Task<IEnumerable<User>> GetUsersPagedAsync(int page, int rpp) =>
    _userRepository.GetUsersPagedAsync(page, rpp);







    }
}

