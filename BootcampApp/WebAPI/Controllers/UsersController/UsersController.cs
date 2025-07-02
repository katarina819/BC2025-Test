using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Common.DTOs;
using BootcampApp.Common.Mappers;
using BootcampApp.Model;    // Model
using BootcampApp.Repository;
using BootcampApp.Service;  // UserService
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppLoginRequest = BootcampApp.Common.LoginRequest.LoginRequest;
using BootcampApp.Common;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller responsible for user management and authentication.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">Service for user operations.</param>
        /// <param name="notificationService">Service for user notifications.</param>
        /// <param name="logger">Logger instance.</param>
        public UsersController(IUserService userService, INotificationService notificationService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _notificationService = notificationService;
            _logger = logger;
        }

        /*
        /// <summary>
        /// Searches users with pagination.
        /// </summary>
        /// <param name="page">Page number (default 1).</param>
        /// <param name="pageSize">Number of users per page (default 10).</param>
        /// <returns>Paged list of users with total count.</returns>
        [HttpGet]
        public async Task<ActionResult> SearchAsync(int page = 1, int pageSize = 10)
        {
            // Implementation...
        }
        */

        /// <summary>
        /// Authenticates user by name and email.
        /// </summary>
        /// <param name="request">Login request containing name and email.</param>
        /// <returns>User data if authentication is successful; Unauthorized otherwise.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppLoginRequest request)
        {
            var user = await _userService.LoginUserAsync(request.Name, request.Email);

            if (user == null)
                return Unauthorized("Invalid name or email.");

            return Ok(user);
        }

        /// <summary>
        /// Retrieves user details by ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>User details if found; NotFound otherwise.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">User creation request containing user info.</param>
        /// <returns>Created user and URI to access it.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                Profile = new UserProfile
                {
                    UserId = null,
                    PhoneNumber = request.Profile.PhoneNumber,
                    Address = request.Profile.Address
                }
            };

            await _userService.CreateUserAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /*
        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="updatedUser">User data to update.</param>
        /// <returns>Updated user data or NotFound.</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, User updatedUser)
        {
            // Implementation...
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <returns>Ok if deleted, NotFound otherwise.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            // Implementation...
        }
        */

        /// <summary>
        /// Updates a user partially with provided data.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="dto">Update data transfer object.</param>
        /// <returns>Updated user data or NotFound if user does not exist.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, dto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves notifications for a specific user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>List of notifications or NotFound if none exist.</returns>
        [HttpGet("{userId}/notifications")]
        public async Task<IActionResult> GetUserNotifications(Guid userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);

            if (notifications == null || !notifications.Any())
                return NotFound("No notifications found for this user.");

            return Ok(notifications);
        }
    }
}
