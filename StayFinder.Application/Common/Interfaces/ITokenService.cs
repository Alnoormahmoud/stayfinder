using StayFinder.Domain.Entities;

namespace StayFinder.Application.Common.Interfaces;

public record AccessTokenResult(string Token, DateTime ExpiresAt);

public interface ITokenService
{
    AccessTokenResult CreateAccessToken(User user);

    // A random, unguessable value given to the client. Only its hash is stored in the database.
    string GenerateRefreshToken();
    string HashToken(string token);

    TimeSpan RefreshTokenLifetime { get; }
}
