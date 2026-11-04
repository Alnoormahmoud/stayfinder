namespace StayFinder.Application.Common.Interfaces;

public interface IAuditService
{
    /// <summary>Writes one row to the audit trail. userId defaults to the current user.</summary>
    Task LogAsync(
        string action,
        Guid? userId = null,
        string? entityType = null,
        Guid? entityId = null,
        string? details = null,
        CancellationToken ct = default);
}
