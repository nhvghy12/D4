using System.Globalization;
using System.Text.Json;

namespace D4;
public class MyCustomMiddleware
{
    private readonly RequestDelegate _next;

    public MyCustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stream = new StreamReader(context.Request.Body);
        var body = await stream.ReadToEndAsync();
        var requestDate = new
        {
            Scheme = context.Request.Scheme,
            Host = context.Request.Host.ToString(),
            Path = context.Request.Path.ToString(),
            QueryString = context.Request.QueryString.ToString(),
            Body = body
        };
        using (StreamWriter writer = File.AppendText("file.txt"))
        {
            var data = JsonSerializer.Serialize(requestDate);
            writer.WriteLine(data);
        }   
        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}
public static class MyCustomMiddlewareExtensions
{
    public static IApplicationBuilder UseMyCustomMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyCustomMiddleware>();
    }
}
