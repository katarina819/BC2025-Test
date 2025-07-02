using System;

namespace BootcampApp.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) used for updating user information.
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the age of the user. Nullable if not provided.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        public string Address { get; set; }
    }
}
