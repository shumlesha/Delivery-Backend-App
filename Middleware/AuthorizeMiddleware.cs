using webNET_Hits_backend_aspnet_project_1.Models;

namespace webNET_Hits_backend_aspnet_project_1.Middleware;

public class AuthorizeMiddleware
{
    private readonly RequestDelegate _next;
  

    public AuthorizeMiddleware(RequestDelegate next)
    {
        _next = next;
       
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        using var scope = httpContext.RequestServices.CreateScope();
        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        
        if (token != null && _context.BannedTokens.Any(tok => tok.TokenString == token))
        {
            httpContext.Response.StatusCode = 401;
            await httpContext.Response.WriteAsync("Unauthorized");
            return;
        }

        await _next(httpContext);
    }
}