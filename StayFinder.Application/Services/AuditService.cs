using StayFinder.Application.Common.Interfaces;
using StayFinder.Domain.Entities;

namespace StayFinder.Application.Services;

public class AuditService(IAppDbContext db, ICurrentUser currentUser) : IAuditService
{
    public async Task LogAsync(
        string action,
        Guid? userId = null,
        string? entityType = null,
        Guid? entityId = null,
        string? details = null,
        CancellationToken ct = default)
    {
        db.AuditLogs.Add(new AuditLog
        {
            UserId = userId ?? currentUser.UserId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details is { Length: > 1000 } ? details[..1000] : details,
            IpAddress = currentUser.IpAddress
        });

        await db.SaveChangesAsync(ct);
    }
}
