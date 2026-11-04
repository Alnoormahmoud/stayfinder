namespace StayFinder.Application.DTOs.Users;

public record UserDto(Guid Id, string FullName, string Email, string? PhoneNumber, string Role);
