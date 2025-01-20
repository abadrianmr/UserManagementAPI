namespace UserManagementAPI.Middlewares;

public static class AuthorizationMiddleware
{
    public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token) || !ValidateToken(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
                return;
            }

            await next(context);
        });
    }

    private static bool ValidateToken(string token)
    {
        return token == "valid-token";
    }
}