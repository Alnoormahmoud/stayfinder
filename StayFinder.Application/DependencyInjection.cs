using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StayFinder.Application.Common.Interfaces;
using StayFinder.Application.Services;

namespace StayFinder.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
