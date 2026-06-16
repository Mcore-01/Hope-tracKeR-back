namespace Hope_tracKeR_back.Middlewares;

public class LogMiddleware
{
    private readonly RequestDelegate _next;

    public LogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var isModifyMethod = context.Request.Method == "POST" ||
                             context.Request.Method == "PUT" ||
                             context.Request.Method == "DELETE";

        var isFilterEndpoint = context.Request.Path.Value?.Contains("/filter") == true;

        if (isModifyMethod && !isFilterEndpoint)
        {
            Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
        }

        await _next(context);

        
        if (isModifyMethod && !isFilterEndpoint)
        {
            Console.WriteLine($"{context.Response.StatusCode} {context.Request.Method} {context.Request.Path}");
        }
    }
}
