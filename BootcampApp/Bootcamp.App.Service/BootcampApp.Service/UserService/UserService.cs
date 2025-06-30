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

        public Task<List<User>> GetAllUsersAsync() => _userRepository.GetAllAsync();


        //public async Task<List<UserDto>> GetAllUsersDtoAsync(string? searchValue, string? sortBy, string sortOrder, int page, int pageSize)
        //{
        //    var users = await _userRepository.SearchAsync(searchValue, sortBy, sortOrder, page, pageSize);

        //    var dtos = users
        //        .Where(u => u.Id != Guid.Empty)  
        //        .Select(u => UserMapper.ToDto(u))
        //        .ToList();

        //    return dtos;
        //}


        public async Task<User?> LoginUserAsync(string name, string email)
        {
            
            return await _userRepository.GetByNameAndEmailAsync(name, email);
        }


        public async Task<UserDto?> GetUserByIdDtoAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.Id == Guid.Empty)
                return null;

            return UserMapper.ToDto(user);
        }


        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }


        public Task<List<User>> GetAllUsersAsync(int page = 1, int rpp = 10)
        {
            _logger.LogInformation("Dohvaćam korisnike - stranica: {Page}, veličina: {PageSize}", page, rpp);
            return _userRepository.GetUsersPagedAsync(page, rpp);
        }



        public Task<Guid> CreateUserAsync(User user) => _userRepository.CreateAsync(user);

        public async Task<bool> UpdateUserDataAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            UpdateUserFromDto(user, dto);

            return await _userRepository.UpdateAsync(id, user);
        }
        public Task<bool> DeleteUserAsync(Guid id) => _userRepository.DeleteAsync(id);

        public async Task<IEnumerable<User>> GetUsersPagedAsync(int page, int pageSize)
        {
            return await _userRepository.GetUsersPagedAsync(page, pageSize);
        }


        public async Task<int> CountFilteredUsersAsync(string? searchValue)
        {
            return await _userRepository.CountFilteredUsersAsync(searchValue);
        }

        public async Task<int> CountUsersAsync()
        {
            return await _userRepository.CountUsersAsync();
        }

        public async Task<List<User>> SearchAsync(int page, int pageSize)
        {
            return await _userRepository.SearchAsync(page, pageSize);
        }

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

        // Ova metoda koristi UpdateUserFromDto i poziva repo za update
        public async Task<User> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                throw new Exception("User not found");

            UpdateUserFromDto(existingUser, dto);

            var updated = await _userRepository.UpdateAsync(id, existingUser);
            if (!updated)
                throw new Exception("Failed to update user");

            return await _userRepository.GetByIdAsync(id); // vrati ažuriranog korisnika
        }

       

        


        








    }
















}


