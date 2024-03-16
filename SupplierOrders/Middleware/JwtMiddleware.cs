namespace SupplierOrders.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Cookies["jwt_token"];

        if (token is not null)
            context.Request.Headers.Add("Authorization", $"Bearer {token}");
        
        await _next(context);
    }
}
