using Microsoft.AspNetCore.Identity;
using StayFinder.Application.Common.Interfaces;
using StayFinder.Domain.Entities;

namespace StayFinder.Infrastructure.Identity;

/// <summary>Password hashing with the PBKDF2 hasher from ASP.NET Core Identity (salted, slow on purpose).</summary>
public class IdentityPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    // The hasher ignores the user object, it only needs a placeholder
    private static readonly User Placeholder = new();

    public string Hash(string password) => _hasher.HashPassword(Placeholder, password);

    public bool Verify(string hash, string password) =>
        _hasher.VerifyHashedPassword(Placeholder, hash, password) != PasswordVerificationResult.Failed;
}
