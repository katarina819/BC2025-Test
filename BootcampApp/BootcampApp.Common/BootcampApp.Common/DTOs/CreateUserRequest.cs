using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty; // 👈 DODAJ
        public string Name { get; set; } = string.Empty;     // 👈 DODAJ
        public string Email { get; set; } = string.Empty;
        public int? Age { get; set; }
        public CreateUserProfileRequest Profile { get; set; }
    }

    public class CreateUserProfileRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }

}
