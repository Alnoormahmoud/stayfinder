namespace StayFinder.Application.Common.Interfaces;

/// <summary>The caller of the current request. Implemented in the API project.</summary>
public interface ICurrentUser
{
    Guid? UserId { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }

    // Used in logs and the audit trail
    string? IpAddress { get; }
}
