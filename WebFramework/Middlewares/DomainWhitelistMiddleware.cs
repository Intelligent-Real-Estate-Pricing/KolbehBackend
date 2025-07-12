using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;


namespace WebFramework.Middlewares
{
    public class DomainWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HashSet<string> _allowedDomains;
        private readonly HashSet<string> _allowedIPs;
        private readonly ILogger<DomainWhitelistMiddleware> _logger;

        public DomainWhitelistMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<DomainWhitelistMiddleware> logger)
        {
            _next = next;
            _logger = logger;

            // خواندن domain های مجاز
            var allowedDomains = configuration.GetSection("AllowedDomains").Get<string[]>() ?? Array.Empty<string>();
            _allowedDomains = allowedDomains.ToHashSet(StringComparer.OrdinalIgnoreCase);

            // خواندن IP های مجاز (برای کارایی بهتر)
            var allowedIPs = configuration.GetSection("AllowedIPs").Get<string[]>() ?? Array.Empty<string>();
            _allowedIPs = allowedIPs.ToHashSet(StringComparer.OrdinalIgnoreCase);

            _logger.LogInformation("Domain Whitelist initialized with {DomainCount} domains and {IPCount} IPs",
                _allowedDomains.Count, _allowedIPs.Count);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIP = GetClientIP(context);
            var origin = context.Request.Headers["Origin"].FirstOrDefault();
            var referer = context.Request.Headers["Referer"].FirstOrDefault();

            // بررسی دسترسی
            if (!await IsAccessAllowed(context, clientIP, origin, referer))
            {
                _logger.LogWarning("🚫 Access denied - IP: {ClientIP}, Origin: {Origin}, Referer: {Referer}",
                    clientIP, origin, referer);

                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Access Denied",
                    message = "Your request origin is not authorized to access this resource",
                    statusCode = 403
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            await _next(context);
        }

        private async Task<bool> IsAccessAllowed(HttpContext context,string clientIP, string origin, string referer)
        {
            // 1. بررسی IP مستقیم
            if (!string.IsNullOrEmpty(clientIP) && _allowedIPs.Contains(clientIP))
            {
                _logger.LogInformation("✅ Access granted by IP: {ClientIP}", clientIP);
                return true;
            }

            // 2. بررسی Origin header
            if (!string.IsNullOrEmpty(origin) && await IsDomainAllowed(origin))
            {
                _logger.LogInformation("✅ Access granted by Origin: {Origin}", origin);
                return true;
            }

            // 3. بررسی Referer header
            if (!string.IsNullOrEmpty(referer) && await IsDomainAllowed(referer))
            {
                _logger.LogInformation("✅ Access granted by Referer: {Referer}", referer);
                return true;
            }

            // 4. بررسی Host header
            var host = context.Request.Host.ToString();
            if (!string.IsNullOrEmpty(host) && _allowedDomains.Contains($"https://{host}"))
            {
                _logger.LogInformation("✅ Access granted by Host: {Host}", host);
                return true;
            }

            return false;
        }

        private async Task<bool> IsDomainAllowed(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return false;

                var uri = new Uri(url);
                var fullUrl = $"{uri.Scheme}://{uri.Host}";

                // بررسی exact match
                if (_allowedDomains.Contains(fullUrl))
                    return true;

                // بررسی با port
                var fullUrlWithPort = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
                if (_allowedDomains.Contains(fullUrlWithPort))
                    return true;

                // DNS lookup برای تبدیل domain به IP
                var hostEntry = await Dns.GetHostEntryAsync(uri.Host);
                foreach (var ip in hostEntry.AddressList)
                {
                    if (_allowedIPs.Contains(ip.ToString()))
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking domain: {Url}", url);
                return false;
            }
        }

        private string GetClientIP(HttpContext context)
        {
            var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                return xForwardedFor.Split(',')[0].Trim();
            }

            var xRealIP = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xRealIP))
            {
                return xRealIP;
            }

            return context.Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}
