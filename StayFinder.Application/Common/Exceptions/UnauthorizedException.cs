namespace StayFinder.Application.Common.Exceptions;

// Mapped to HTTP 401 by the exception-handling middleware in the API project
public class UnauthorizedException(string message) : Exception(message)
{
}
