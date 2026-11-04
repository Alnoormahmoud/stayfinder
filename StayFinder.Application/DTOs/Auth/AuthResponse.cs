using StayFinder.Application.DTOs.Users;

namespace StayFinder.Application.DTOs.Auth;

public record AuthResponse(string AccessToken, DateTime AccessTokenExpiresAt, string RefreshToken, UserDto User);
