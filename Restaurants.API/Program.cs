using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Host.UseSerilog((context, configuraion) =>
    configuraion.ReadFrom.Configuration(context.Configuration)

//configuraion
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
//    .WriteTo.File("Logs/Restaurant-Api-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
//    .WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine} {Message:lj}{NewLine}{Exception}")
);

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

await seeder.Seed();
// Configure the HTTP request pipeline.
//app.UseSerilogRequestLogging();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
