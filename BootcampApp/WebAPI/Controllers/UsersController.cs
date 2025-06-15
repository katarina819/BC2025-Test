using BootcampApp.Model;    // Model
using BootcampApp.Service;  // UserService
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
            string? searchValue = null,
            string? sortBy = null,
            int page = 1, 
            int rpp = 10)
        {
            if (page < 1) page = 1;
            if (rpp < 1) rpp = 10;

            try
            {
                var users = await _userService.GetAllUsersAsync(searchValue, sortBy, page, rpp);

                var userDtos = users.Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Age = user.Age,
                    Profile = user.Profile == null ? null : new UserProfileDto
                    {
                        UserId = user.Profile.UserId,
                        PhoneNumber = user.Profile.PhoneNumber,
                        Address = user.Profile.Address
                    }
                });

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri dohvaćanju korisnika.");
                return StatusCode(500, "Interna greška servera.");
            }
        }



        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound();

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Age = user.Age,
                    Profile = user.Profile == null ? null : new UserProfileDto
                    {
                        UserId = user.Profile.UserId,
                        PhoneNumber = user.Profile.PhoneNumber,
                        Address = user.Profile.Address
                    }
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri dohvaćanju korisnika s ID {UserId}", id);
                return StatusCode(500, "Interna greška servera.");
            }
        }


        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User newUser)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email))
                    return BadRequest("Name and Email are required.");

                var createdId = await _userService.CreateUserAsync(newUser);
                newUser.Id = createdId;

                return CreatedAtAction(nameof(GetUser), new { id = createdId }, newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju korisnika.");
                return StatusCode(500, "Interna greška servera.");
            }
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, User updatedUser)
        {
            try
            {
                var updated = await _userService.UpdateUserAsync(id, updatedUser);
                return updated ? Ok(updatedUser) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju korisnika s ID {UserId}", id);
                return StatusCode(500, "Interna greška servera.");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var deleted = await _userService.DeleteUserAsync(id);
                return deleted ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri brisanju korisnika s ID {UserId}", id);
                return StatusCode(500, "Interna greška servera.");
            }



        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers(
    string? searchValue,
    string? sortBy,
    int page = 1,
    int rpp = 10)
        {
            var users = await _userService.GetAllUsersAsync(searchValue, sortBy, page, rpp);

            var dtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Age = u.Age,
                Profile = u.Profile == null ? null : new UserProfileDto
                {
                    UserId = u.Id,
                    PhoneNumber = u.Profile.PhoneNumber,
                    Address = u.Profile.Address
                }
            });

            return Ok(dtos);
        }
    }
}

