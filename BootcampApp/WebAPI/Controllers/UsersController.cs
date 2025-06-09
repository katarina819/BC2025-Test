//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;



//namespace WebAPI.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class UsersController : ControllerBase
//{
//    private static List<User> users = new()
//    {
//        new User { Id = 1, Name = "Ana", Email = "ana@example.com" },
//        new User { Id = 2, Name = "Ivan", Email = "ivan@example.com" },
//        new User { Id = 3, Name = "Marko", Email = "marko@example.com" },
//        new User { Id = 4, Name = "Petra", Email = "petra@example.com" },
//        new User { Id = 5, Name = "Luka", Email = "luka@example.com" },
//        new User { Id = 6, Name = "Maja", Email = "maja@example.com" },
//        new User { Id = 7, Name = "Nikola", Email = "nikola@example.com" },
//        new User { Id = 8, Name = "Tina", Email = "tina@example.com" },
//        new User { Id = 9, Name = "Filip", Email = "filip@example.com" },
//        new User { Id = 10, Name = "Ivana", Email = "ivana@example.com" }
//    };

//    [HttpGet]
//    public ActionResult<IEnumerable<User>> GetUsers()
//    {
//        var shuffled = users.OrderBy(u => Guid.NewGuid()).ToList();
//        return Ok(shuffled);
//    }


//    [HttpGet("{id}")]
//    public ActionResult<User> GetUser(int id)
//    {
//        var user = users.FirstOrDefault(u => u.Id == id);
//        return user != null ? Ok(user) : NotFound();
//    }

//    [HttpPost]
//    [ProducesResponseType(StatusCodes.Status201Created)]
//    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
//    public ActionResult<User> CreateUser(User newUser)
//    {
//        if (string.IsNullOrEmpty(newUser.Name) || string.IsNullOrEmpty(newUser.Email))
//        {
//            return BadRequest("Name and Email are required.");
//        }

//        newUser.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
//        users.Add(newUser);
//        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
//    }


//    [HttpPut("{id}")]
//    [ProducesResponseType(StatusCodes.Status200OK)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status400BadRequest)]
//    public IActionResult UpdateUser(int id, User updatedUser)
//    {
//        if (id == 9 || id == 10)
//            return BadRequest("Korisnici s ID 9 i 10 se ne mogu mijenjati.");

//        var user = users.FirstOrDefault(u => u.Id == id);
//        if (user == null) return NotFound();

//        user.Name = updatedUser.Name;
//        user.Email = updatedUser.Email;
//        return Ok(user);
//    }



//    [HttpDelete("{id}")]
//    [ProducesResponseType(StatusCodes.Status200OK)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status400BadRequest)]
//    public IActionResult DeleteUser(int id)
//    {
//        if (id == 9 || id == 10)
//            return BadRequest("Brisanje korisnika s ID 9 i 10 nije dozvoljeno.");

//        var user = users.FirstOrDefault(u => u.Id == id);
//        if (user == null) return NotFound();

//        users.Remove(user);
//        return Ok();
//    }

//    [HttpGet("reset")]
//    public IActionResult ResetUsers()
//    {
//        users = new List<User>
//    {
//        new User { Id = 1, Name = "Ana", Email = "ana@example.com" },
//        new User { Id = 2, Name = "Ivan", Email = "ivan@example.com" },
//        new User { Id = 3, Name = "Marko", Email = "marko@example.com" },
//        new User { Id = 4, Name = "Petra", Email = "petra@example.com" },
//        new User { Id = 5, Name = "Luka", Email = "luka@example.com" },
//        new User { Id = 6, Name = "Maja", Email = "maja@example.com" },
//        new User { Id = 7, Name = "Nikola", Email = "nikola@example.com" },
//        new User { Id = 8, Name = "Tina", Email = "tina@example.com" },
//        new User { Id = 9, Name = "Filip", Email = "filip@example.com" },
//        new User { Id = 10, Name = "Ivana", Email = "ivana@example.com" }
//    };

//        return Ok("Lista korisnika je resetirana.");
//    }





//}

using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user != null ? Ok(user) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User newUser)
    {
        if (string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email))
        {
            return BadRequest("Name and Email are required.");
        }

        var createdId = await _userService.CreateUserAsync(newUser);
        return CreatedAtAction(nameof(GetUser), new { id = createdId }, newUser);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, User updatedUser)
    {
        var updated = await _userService.UpdateUserAsync(id, updatedUser);
        return updated ? Ok(updatedUser) : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var deleted = await _userService.DeleteUserAsync(id);
        return deleted ? Ok() : NotFound();
    }
}


