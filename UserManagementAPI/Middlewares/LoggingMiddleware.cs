namespace UserManagementAPI.Middlewares;

public static class LoggingMiddleware
{
    public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            // Log the request details
            logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

            // Call the next middleware in the pipeline
            await next(context);

            // Log the response details
            logger.LogInformation($"Outgoing response: {context.Response.StatusCode}");
        });
    }
}