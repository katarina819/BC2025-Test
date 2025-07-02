using System;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a user associated with a drink order.
    /// </summary>
    public class DrinkOrderUserDto
    {
        /// <summary>
        /// Gets or sets the profile details of the user.
        /// </summary>
        public DrinkOrderUserProfileDto Profile { get; set; } = default!;
    }
}
