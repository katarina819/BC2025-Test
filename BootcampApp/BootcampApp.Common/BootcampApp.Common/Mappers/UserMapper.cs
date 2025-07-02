using System.Collections.Generic;
using System.Linq;
using BootcampApp.Model;
using BootcampApp.Common.DTOs;

namespace BootcampApp.Common.Mappers
{
    /// <summary>
    /// Provides mapping methods to convert User entities to UserDto objects.
    /// </summary>
    public static class UserMapper
    {
        /// <summary>
        /// Maps a User entity to a UserDto.
        /// </summary>
        /// <param name="user">The User entity to map.</param>
        /// <returns>A UserDto representing the given User entity. Returns null if input is null.</returns>
        public static UserDto ToDto(User user)
        {
            if (user == null) return null!;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                Profile = user.Profile != null
                    ? new UserProfileDto
                    {
                        UserId = user.Profile.UserId,
                        PhoneNumber = user.Profile.PhoneNumber,
                        Address = user.Profile.Address
                    }
                    : null
            };
        }

        /// <summary>
        /// Maps a collection of User entities to a collection of UserDto objects.
        /// </summary>
        /// <param name="users">The collection of User entities to map.</param>
        /// <returns>An IEnumerable of UserDto objects.</returns>
        public static IEnumerable<UserDto> ToDtoList(IEnumerable<User> users)
        {
            return users.Select(ToDto);
        }
    }
}
