namespace StayFinder.Application.Common.Exceptions;

// Mapped to HTTP 409 by the exception-handling middleware in the API project
public class ConflictException(string message) : Exception(message)
{
}
