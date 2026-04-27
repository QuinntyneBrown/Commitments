namespace Commitments.Api.Middleware;

public sealed class JwtQueryStringAuthMiddleware
{
    private readonly RequestDelegate _next;

    public JwtQueryStringAuthMiddleware(RequestDelegate next) => _next = next;

    public Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/hub"))
        {
            var headers = context.Request.Headers;
            var query = context.Request.Query;

            if (!headers.ContainsKey("Authorization") && query.TryGetValue("token", out var token))
                headers["Authorization"] = $"Bearer {token}";

            if (!headers.ContainsKey("ProfileId") && query.TryGetValue("profileId", out var profileId))
                headers["ProfileId"] = profileId.ToString();
        }
        return _next(context);
    }
}
