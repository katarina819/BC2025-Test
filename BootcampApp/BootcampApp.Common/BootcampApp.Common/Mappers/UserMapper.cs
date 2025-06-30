using System.Collections.Generic;
using System.Linq;
using BootcampApp.Model; 
using BootcampApp.Common.DTOs;    

namespace BootcampApp.Common.Mappers
{
    public static class UserMapper
    {
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

        public static IEnumerable<UserDto> ToDtoList(IEnumerable<User> users)
        {
            return users.Select(ToDto);
        }
    }
}

