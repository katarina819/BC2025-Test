using System;

namespace BootcampApp.Common.LoginRequest
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a login request.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets the username or name used for login.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address used for login.
        /// </summary>
        public string Email { get; set; }
    }
}
