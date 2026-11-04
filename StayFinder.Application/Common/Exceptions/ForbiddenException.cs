namespace StayFinder.Application.Common.Exceptions;

// Mapped to HTTP 403 by the exception-handling middleware in the API project
public class ForbiddenException(string message) : Exception(message)
{
}
