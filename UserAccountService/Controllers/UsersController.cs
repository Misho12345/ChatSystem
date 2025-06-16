using UserAccountService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAccountService.Models.DTOs;

namespace UserAccountService.Controllers;

/// <summary>
/// Controller for managing user-related operations.
/// Requires authorization for all endpoints.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Retrieves the currently authenticated user's information.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the user's information if found,
    /// or an appropriate error response (Unauthorized or NotFound).
    /// </returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var user = await userService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();
        return Ok(new UserDto(user));
    }

    /// <summary>
    /// Retrieves a user's information by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the user's information if found,
    /// or a NotFound response if the user does not exist.
    /// </returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(new UserDto(user));
    }

    /// <summary>
    /// Searches for users based on a query string.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of users matching the query,
    /// or a BadRequest response if the query is empty.
    /// </returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return BadRequest("Search query cannot be empty.");
        var users = await userService.SearchUsersAsync(query);
        return Ok(users.Select(u => new UserDto(u)));
    }
}