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
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserService userService, INotificationService notificationService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _notificationService = notificationService;
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<ActionResult> SearchAsync(int page = 1, int pageSize = 10)
        //{
        //    var users = await _userService.GetUsersPagedAsync(page, pageSize);
        //    var totalCount = await _userService.CountUsersAsync();

        //    var result = new
        //    {
        //        Data = users,
        //        TotalCount = totalCount,
        //        Page = page,
        //        RowsPerPage = pageSize
        //    };

        //    return Ok(result);
        //}


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppLoginRequest request)
        {
            var user = await _userService.LoginUserAsync(request.Name, request.Email);

            if (user == null)
                return Unauthorized("Invalid name or email.");

            return Ok(user);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }




        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),             
                Name = request.Name,         // ✅ sad postoji
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

        //[HttpPut("{id:guid}")]
        //public async Task<IActionResult> UpdateUser(Guid id, User updatedUser)
        //{
        //    try
        //    {
        //        var updated = await _userService.UpdateUserAsync(id, updatedUser);
        //        return updated ? Ok(updatedUser) : NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Greška pri ažuriranju korisnika s ID {UserId}", id);
        //        return StatusCode(500, "Interna greška servera.");
        //    }
        //}

        //[HttpDelete("{id:guid}")]
        //public async Task<IActionResult> DeleteUser(Guid id)
        //{
        //    try
        //    {
        //        var deleted = await _userService.DeleteUserAsync(id);
        //        return deleted ? Ok() : NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Greška pri brisanju korisnika s ID {UserId}", id);
        //        return StatusCode(500, "Interna greška servera.");
        //    }



        //}

        //[HttpGet("search")]
        //public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers(string? searchValue, string? sortBy, string sortOrder = "asc", int page = 1, int rpp = 10)
        //{
        //    var userDtos = await _userService.SearchAsync(searchValue, sortBy,  sortOrder, page, rpp);
        //    return Ok(userDtos);
        //}



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

