namespace StayFinder.API.Security;

/// <summary>Names of the authorization policies. Use them like [Authorize(Policy = Policies.RequireHost)].</summary>
public static class Policies
{
    public const string RequireHost = "RequireHost";   // Host or Admin
    public const string RequireAdmin = "RequireAdmin"; // Admin only
}
