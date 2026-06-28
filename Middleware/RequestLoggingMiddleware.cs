using System.Diagnostics;

namespace CiCd.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

        var path = context.Request.Path;
        if (context.Request.QueryString.HasValue)
        {
            var safeQuery = SanitizeQueryString(context.Request.QueryString.Value ?? "");
            path = $"{context.Request.Path}{safeQuery}";
        }

        _logger.LogInformation("--> {Method} {Path} [trace={TraceId}]", context.Request.Method, path, traceId);

        try
        {
            await _next(context);
            sw.Stop();

            _logger.LogInformation(
                "<-- {Method} {Path} {StatusCode} {ElapsedMs}ms [trace={TraceId}]",
                context.Request.Method, path, context.Response.StatusCode, (int)sw.Elapsed.TotalMilliseconds, traceId);
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(
                ex,
                "<-- {Method} {Path} {StatusCode} {ElapsedMs}ms [trace={TraceId}]",
                context.Request.Method, path, 500, (int)sw.Elapsed.TotalMilliseconds, traceId);
            throw;
        }
    }

    private static string SanitizeQueryString(string queryString)
    {
        if (string.IsNullOrEmpty(queryString)) return "";

        var parts = System.Web.HttpUtility.ParseQueryString(queryString.Remove(0, 1));
        foreach (string? key in parts.AllKeys)
        {
            if (key != null && IsSensitiveKey(key))
            {
                parts[key] = "***REDACTED***";
            }
        }
        var sanitized = parts.ToString();
        return string.IsNullOrEmpty(sanitized) ? "" : "?" + sanitized;
    }

    private static bool IsSensitiveKey(string key)
    {
        var lower = key.ToLowerInvariant();
        return lower.Contains("password") || lower.Contains("secret") ||
               lower.Contains("token") || lower.Contains("key") ||
               lower.Contains("auth");
    }
}
