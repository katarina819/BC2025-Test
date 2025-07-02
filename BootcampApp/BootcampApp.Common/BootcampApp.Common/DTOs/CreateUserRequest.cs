using System;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    /// <summary>
    /// Represents a request to create a new user.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the age of the user. Nullable if not provided.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the profile information of the user.
        /// </summary>
        public CreateUserProfileRequest Profile { get; set; }
    }

    /// <summary>
    /// Represents the profile details of a user.
    /// </summary>
    public class CreateUserProfileRequest
    {
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
