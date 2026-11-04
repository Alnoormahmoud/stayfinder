using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayFinder.Application.Common.Exceptions;

namespace StayFinder.API.Middleware;

/// <summary>Turns exceptions into clean JSON error responses (ProblemDetails) with the right status code.</summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception ex)
    {
        var problem = new ProblemDetails { Instance = context.Request.Path };

        switch (ex)
        {
            case FluentValidation.ValidationException validation:
                problem.Status = StatusCodes.Status400BadRequest;
                problem.Title = "Validation failed";
                problem.Extensions["errors"] = validation.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                break;

            case NotFoundException:
                problem.Status = StatusCodes.Status404NotFound;
                problem.Title = "Not found";
                problem.Detail = ex.Message;
                break;

            case ConflictException:
                problem.Status = StatusCodes.Status409Conflict;
                problem.Title = "Conflict";
                problem.Detail = ex.Message;
                break;

            case UnauthorizedException:
                problem.Status = StatusCodes.Status401Unauthorized;
                problem.Title = "Unauthorized";
                problem.Detail = ex.Message;
                break;

            case ForbiddenException:
                problem.Status = StatusCodes.Status403Forbidden;
                problem.Title = "Forbidden";
                problem.Detail = ex.Message;
                break;

            case DbUpdateConcurrencyException:
                problem.Status = StatusCodes.Status409Conflict;
                problem.Title = "Conflict";
                problem.Detail = "The record was changed by someone else. Reload it and try again.";
                break;

            default:
                logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);
                problem.Status = StatusCodes.Status500InternalServerError;
                problem.Title = "An unexpected error occurred";
                break;
        }

        context.Response.StatusCode = problem.Status!.Value;
        await context.Response.WriteAsJsonAsync(problem);
    }
}
