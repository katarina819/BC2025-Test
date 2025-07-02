namespace BootcampApp.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a user.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the age of the user. Nullable if not provided.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the profile details of the user. Nullable.
        /// </summary>
        public UserProfileDto? Profile { get; set; }
    }

    /// <summary>
    /// Data Transfer Object (DTO) representing a user's profile information.
    /// </summary>
    public class UserProfileDto
    {
        /// <summary>
        /// Gets or sets the ID of the user associated with this profile. Nullable.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user. Nullable.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address of the user. Nullable.
        /// </summary>
        public string? Address { get; set; }
    }
}
