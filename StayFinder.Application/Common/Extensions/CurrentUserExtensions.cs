using StayFinder.Application.Common.Exceptions;
using StayFinder.Application.Common.Interfaces;
using StayFinder.Domain.Enums;

namespace StayFinder.Application.Common.Extensions;

public static class CurrentUserExtensions
{
    public static bool IsAdmin(this ICurrentUser user) => user.Role == nameof(UserRole.Admin);

    /// <summary>
    /// Ownership rule: the caller must own the resource, or be an admin.
    /// Stops one user from reading or changing another user's data (horizontal privilege escalation).
    /// </summary>
    public static void EnsureOwnerOrAdmin(this ICurrentUser user, Guid ownerId)
    {
        if (user.UserId is null)
            throw new UnauthorizedException("You must be logged in.");

        if (user.UserId != ownerId && !user.IsAdmin())
            throw new ForbiddenException("You do not have access to this resource.");
    }
}
