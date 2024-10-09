using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionManagementAPI.Models;
using AuctionManagementAPI.Models.Dto;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UserController> _logger;

    public UserController(ApplicationDbContext dbContext, ILogger<UserController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    // GET: api/User
    [HttpGet]
    public ActionResult<IEnumerable<UserDTO>> GetAllUsers()
    {
        _logger.LogInformation("GetAllUsers method called");

        var users = _dbContext.Users
                              .Select(u => new UserDTO
                              {
                                  UserId = u.UserId,
                                  Username = u.Username,
                                  Email = u.Email,
                                  Role = u.Role // Added the Role field to the DTO
                              }).ToList();

        return Ok(users);
    }

    // GET: api/User/{UserId}
    [HttpGet("{UserId:int}", Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserDTO> GetUser(int UserId)
    {
        if (UserId == 0)
        {
            _logger.LogError("Invalid UserId provided: " + UserId);
            return BadRequest();
        }

        var user = _dbContext.Users.FirstOrDefault(u => u.UserId == UserId);

        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role // Mapped the Role field
        };

        return Ok(userDto);
    }

    // POST: api/User
    

[HttpPost]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDto)
{
    if (userDto == null || !ModelState.IsValid)
    {
        _logger.LogError("Invalid user data: {ModelState}", ModelState);
        return BadRequest(ModelState);
    }

    if (_dbContext.Users.Any(u => u.Username == userDto.Username || u.Email == userDto.Email))
    {
        return Conflict("A user with the same username or email already exists.");
    }

    // Hash the password
    var passwordHasher = new PasswordHasher<UserDTO>();
    var passwordHash = passwordHasher.HashPassword(userDto, userDto.Password);

    var user = new User
    {
        Username = userDto.Username,
        Email = userDto.Email,
        Role = userDto.Role,
        PasswordHash = passwordHash // Set the hashed password here
    };

    try
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        _logger.LogError(ex, "An error occurred while saving to the database");
        return StatusCode(StatusCodes.Status500InternalServerError, "Database error");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred");
        return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
    }

    return CreatedAtRoute("GetUser", new { UserId = user.UserId }, userDto);
}


// PUT: api/User/{id}
[HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDto)
    {
        if (id != userDto.UserId || userDto == null)
        {
            return BadRequest();
        }

        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Update fields
        user.Username = userDto.Username;
        user.Email = userDto.Email;
        user.Role = userDto.Role; // Make sure Role gets updated too

        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/User/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
