using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Users;

namespace Restaurants.Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServiceExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));

        services.AddAutoMapper(assembly);

        services.AddValidatorsFromAssembly(assembly)
            .AddFluentValidationAutoValidation();

        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}
