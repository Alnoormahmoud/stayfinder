namespace StayFinder.Application.DTOs.Auth;

// Used by both the refresh and the logout endpoints
public record RefreshTokenRequest(string RefreshToken);
