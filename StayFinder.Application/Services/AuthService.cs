using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StayFinder.Application.Common;
using StayFinder.Application.Common.Exceptions;
using StayFinder.Application.Common.Interfaces;
using StayFinder.Application.DTOs.Auth;
using StayFinder.Application.DTOs.Users;
using StayFinder.Domain.Entities;
using StayFinder.Domain.Enums;

namespace StayFinder.Application.Services;

public class AuthService(
    IAppDbContext db,
    IPasswordHasher hasher,
    ITokenService tokens,
    IAuditService audit,
    ICurrentUser currentUser,
    ILogger<AuthService> logger,
    IValidator<RegisterRequest> registerValidator,
    IValidator<LoginRequest> loginValidator) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        await registerValidator.ValidateAndThrowAsync(request, ct);

        var email = NormalizeEmail(request.Email);
        if (await db.Users.AnyAsync(u => u.Email == email, ct))
            throw new ConflictException("An account with this email already exists.");

        // The role is never read from the request: everyone starts as a Guest
        var user = new User
        {
            Id = Guid.CreateVersion7(),
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = hasher.Hash(request.Password),
            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim(),
            Role = UserRole.Guest
        };

        db.Users.Add(user);

        // Saves the user and the first refresh token in one go
        var response = await IssueTokensAsync(user, ct);

        logger.LogInformation("User registered. UserId={UserId}", user.Id);
        await audit.LogAsync(AuditActions.UserRegistered, user.Id, nameof(User), user.Id, ct: ct);

        return response;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        await loginValidator.ValidateAndThrowAsync(request, ct);

        var email = NormalizeEmail(request.Email);
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        // Same message for "no such user" and "wrong password" so attackers cannot probe emails
        if (user is null || !hasher.Verify(user.PasswordHash, request.Password))
        {
            logger.LogWarning(
                "Failed login attempt. Account={Account} Ip={Ip}",
                LogSanitizer.MaskEmail(email), currentUser.IpAddress);

            await audit.LogAsync(AuditActions.LoginFailed, user?.Id, nameof(User), user?.Id, ct: ct);
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            logger.LogWarning("Login blocked, account disabled. UserId={UserId} Ip={Ip}", user.Id, currentUser.IpAddress);
            await audit.LogAsync(AuditActions.LoginBlockedDisabledAccount, user.Id, nameof(User), user.Id, ct: ct);
            throw new ForbiddenException("This account has been disabled.");
        }

        var response = await IssueTokensAsync(user, ct);

        logger.LogInformation("Login succeeded. UserId={UserId}", user.Id);
        await audit.LogAsync(AuditActions.LoginSucceeded, user.Id, nameof(User), user.Id, ct: ct);

        return response;
    }

    public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var hash = tokens.HashToken(request.RefreshToken);

        var stored = await db.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.TokenHash == hash, ct);

        if (stored is null)
            throw new UnauthorizedException("Invalid refresh token.");

        if (stored.RevokedAt is not null)
        {
            // A token that was already rotated is being used again: it was probably stolen.
            // Revoke every active session of this user.
            if (stored.ReplacedByTokenHash is not null)
            {
                await RevokeAllActiveTokensAsync(stored.UserId, ct);

                logger.LogWarning(
                    "SECURITY: refresh token reuse detected, all sessions revoked. UserId={UserId} Ip={Ip}",
                    stored.UserId, currentUser.IpAddress);
                await audit.LogAsync(AuditActions.RefreshTokenReuseDetected, stored.UserId, nameof(User), stored.UserId, ct: ct);
            }

            throw new UnauthorizedException("Refresh token is no longer valid.");
        }

        if (stored.ExpiresAt <= DateTime.UtcNow || !stored.User.IsActive)
            throw new UnauthorizedException("Refresh token has expired.");

        // Rotate: the old token dies, a new one is issued
        var newRaw = tokens.GenerateRefreshToken();
        var newHash = tokens.HashToken(newRaw);

        stored.RevokedAt = DateTime.UtcNow;
        stored.ReplacedByTokenHash = newHash;

        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = stored.UserId,
            TokenHash = newHash,
            ExpiresAt = DateTime.UtcNow.Add(tokens.RefreshTokenLifetime)
        });

        var access = tokens.CreateAccessToken(stored.User);
        await db.SaveChangesAsync(ct);

        await audit.LogAsync(AuditActions.TokenRefreshed, stored.UserId, nameof(User), stored.UserId, ct: ct);

        return new AuthResponse(access.Token, access.ExpiresAt, newRaw, stored.User.ToDto());
    }

    public async Task LogoutAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var hash = tokens.HashToken(request.RefreshToken);
        var stored = await db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash, ct);

        if (stored is not null && stored.RevokedAt is null)
        {
            stored.RevokedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);

            await audit.LogAsync(AuditActions.LoggedOut, stored.UserId, nameof(User), stored.UserId, ct: ct);
        }
    }

    private async Task<AuthResponse> IssueTokensAsync(User user, CancellationToken ct)
    {
        var access = tokens.CreateAccessToken(user);
        var rawRefresh = tokens.GenerateRefreshToken();

        db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = tokens.HashToken(rawRefresh),
            ExpiresAt = DateTime.UtcNow.Add(tokens.RefreshTokenLifetime)
        });

        await db.SaveChangesAsync(ct);

        return new AuthResponse(access.Token, access.ExpiresAt, rawRefresh, user.ToDto());
    }

    private async Task RevokeAllActiveTokensAsync(Guid userId, CancellationToken ct)
    {
        var active = await db.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ToListAsync(ct);

        foreach (var token in active)
            token.RevokedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}
