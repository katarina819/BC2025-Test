


namespace BootcampApp.Common.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }  

        public UserProfileDto? Profile { get; set; }  
    }

    public class UserProfileDto
    {
        public Guid? UserId { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}

