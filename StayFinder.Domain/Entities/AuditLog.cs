namespace StayFinder.Domain.Entities;

// Security and admin history: who did what, when and from where.
// UserId is a plain column (no foreign key) so the history survives even if a user is removed.
public class AuditLog
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Action { get; set; } = default!;
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string? Details { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
