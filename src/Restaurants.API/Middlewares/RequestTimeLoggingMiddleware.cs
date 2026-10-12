namespace Restaurants.API.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger)
    : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            var startTime = DateTime.UtcNow;
            context.Response.OnStarting(() =>
            {
                var elapsedTime = DateTime.UtcNow - startTime;
                var logMessage = @$"Request to {context.Request.Path} with verb {context.Request.Method}
                                    took {elapsedTime.TotalMilliseconds} ms.";

                logger.LogInformation(logMessage);
                return Task.CompletedTask;
            });
            return next(context);

        }
        catch (Exception ex)
        {
            logger.LogError($"An error occurred {ex.Message}");
            throw;
        }
    }
}
