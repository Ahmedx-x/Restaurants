using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");

        services.AddDbContext<RestaurantsDbContext>(options =>
            options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());

        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();

        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

        services.AddScoped<IDishesRepository, DishesRepository>();

        return services;
    }
}
