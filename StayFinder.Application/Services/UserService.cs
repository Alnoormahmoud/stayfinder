using Microsoft.EntityFrameworkCore;
using StayFinder.Application.Common.Exceptions;
using StayFinder.Application.Common.Interfaces;
using StayFinder.Application.DTOs.Users;

namespace StayFinder.Application.Services;

public class UserService(IAppDbContext db) : IUserService
{
    public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct)
            ?? throw new NotFoundException("User not found.");

        return user.ToDto();
    }
}
