using System.Security.Claims;
using StayFinder.Application.Common.Interfaces;

namespace StayFinder.API.Services;

public class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public Guid? UserId =>
        Guid.TryParse(Principal?.FindFirst("sub")?.Value, out var id) ? id : null;

    public string? Role => Principal?.FindFirst("role")?.Value;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public string? IpAddress => accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
}
