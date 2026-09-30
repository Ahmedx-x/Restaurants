using Microsoft.OpenApi.Models;
using Restaurants.API.Middlewares;
using Serilog;

namespace Restaurants.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        []
                    }
                });
            });

            builder.Host.UseSerilog((context, configuraion) =>
                configuraion.ReadFrom.Configuration(context.Configuration)

            //configuraion
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            //    .WriteTo.File("Logs/Restaurant-Api-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            //    .WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine} {Message:lj}{NewLine}{Exception}")
            );

            return builder;
        }
    }
}
