using System.Security.Claims;
using CementFactoryAdmin.Constants;
using CementFactoryAdmin.Services;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;

    public RoleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AuthService authService)
    {
        // Check if the user is authenticated
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            // Redirect to login page if not authenticated
            context.Response.Redirect("/auth/login");
            return;
        }

        // Extract user roles from claims
        var userRoles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        // Check if user is accessing admin area
        if (context.Request.Path.StartsWithSegments("/admin") && !userRoles.Contains(Roles.Admin))
        {
            // Redirect to a "Not Authorized" page or return a 403 error if user doesn't have admin role
            context.Response.Redirect("/auth/login");
            return;
        }

        // Check if user is accessing operator area
        if (context.Request.Path.StartsWithSegments("/operator") && !userRoles.Contains(Roles.Operator))
        {
            // Redirect to a "Not Authorized" page or return a 403 error if user doesn't have operator role
            context.Response.Redirect("/auth/login");
            return;
        }

        // Allow the request to proceed if authentication and authorization succeed
        await _next(context);
    }
}