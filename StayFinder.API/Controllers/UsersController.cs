using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFinder.Application.Common.Exceptions;
using StayFinder.Application.Common.Interfaces;
using StayFinder.Application.DTOs.Users;
using StayFinder.Application.Services;

namespace StayFinder.API.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController(IUserService users, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe(CancellationToken ct)
    {
        var id = currentUser.UserId ?? throw new UnauthorizedException("You must be logged in.");
        return Ok(await users.GetByIdAsync(id, ct));
    }
}
