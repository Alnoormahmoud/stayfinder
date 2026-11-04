using StayFinder.Domain.Entities;

namespace StayFinder.Application.DTOs.Users;

public static class UserMappings
{
    public static UserDto ToDto(this User user) =>
        new(user.Id, user.FullName, user.Email, user.PhoneNumber, user.Role.ToString());
}
