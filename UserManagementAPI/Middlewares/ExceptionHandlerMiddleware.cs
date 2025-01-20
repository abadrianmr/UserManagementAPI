namespace UserManagementAPI.Middlewares;

public static class ExceptionHandlerMiddleware
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch
            {
                context.Response.StatusCode = 500;
                await context
                    .Response
                    .WriteAsJsonAsync(
                        new { error = "Internal server error." });
            }
        });
    }
}