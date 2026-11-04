namespace StayFinder.Application.Common.Exceptions;

// Mapped to HTTP 404 by the exception-handling middleware in the API project
public class NotFoundException(string message) : Exception(message)
{
}
