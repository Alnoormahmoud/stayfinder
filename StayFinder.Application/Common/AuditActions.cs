namespace StayFinder.Application.Common;

public static class AuditActions
{
    public const string UserRegistered = "UserRegistered";
    public const string LoginSucceeded = "LoginSucceeded";
    public const string LoginFailed = "LoginFailed";
    public const string LoginBlockedDisabledAccount = "LoginBlockedDisabledAccount";
    public const string TokenRefreshed = "TokenRefreshed";
    public const string RefreshTokenReuseDetected = "RefreshTokenReuseDetected";
    public const string LoggedOut = "LoggedOut";
}
