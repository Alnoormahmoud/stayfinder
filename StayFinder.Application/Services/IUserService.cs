using StayFinder.Application.DTOs.Users;

namespace StayFinder.Application.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct = default);
}
