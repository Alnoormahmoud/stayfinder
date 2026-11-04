namespace StayFinder.Application.Common;

/// <summary>Helpers that keep personal data out of the logs.</summary>
public static class LogSanitizer
{
    // "alnoor@example.com" becomes "a***@example.com"
    public static string MaskEmail(string email)
    {
        var at = email.IndexOf('@');
        return at <= 1 ? "***" : $"{email[0]}***{email[at..]}";
    }
}
