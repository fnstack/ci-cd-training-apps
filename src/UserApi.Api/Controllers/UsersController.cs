using Microsoft.AspNetCore.Mvc;
using UserApi.Api.Models;
using UserApi.Api.Services;

namespace UserApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(new { users });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(new { error = "User not found" });
        }

        return Ok(new { user });
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { error = "Name and email are required" });
        }

        if (!request.Email.Contains('@'))
        {
            return BadRequest(new { error = "Invalid email format" });
        }

        try
        {
            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { user });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<User>> UpdateUser(int id, CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { error = "Name and email are required" });
        }

        if (!request.Email.Contains('@'))
        {
            return BadRequest(new { error = "Invalid email format" });
        }

        try
        {
            var user = await _userService.UpdateUserAsync(id, request);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new { user });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var deleted = await _userService.DeleteUserAsync(id);
        if (!deleted)
        {
            return NotFound(new { error = "User not found" });
        }

        return Ok(new { message = "User deleted successfully" });
    }
}