using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using System.Text.Json;
using StackExchange.Redis;
using Serilog;
using Common;
using WebFramework.CustomMapping;
using WebFramework.Configuration;
using IdGen.DependencyInjection;
using WebFramework.Swagger;
using WebFramework.Middlewares;
using Services.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configuration Setup
var configuration = builder.Configuration;
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
builder.Host.UseSerilog(SerilogConfig.ConfigureLogger);

// Settings Configuration
var siteSettings = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>()

    ?? throw new InvalidOperationException("SiteSettings configuration is required");
builder.Services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));

// Core Services
builder.Services.AddSignalR(ConfigureSignalR);
builder.Services.InitializeAutoMapper();
builder.Services.AddDbContext(configuration);

builder.Services.AddCustomIdentity(siteSettings.IdentitySettings);
builder.Services.AddMinimalMvc();
builder.Services.AddJwtAuthentication(siteSettings.JwtSettings);
builder.Services.AddServices();
builder.Services.AddCustomApiVersioning();

builder.Services.AddIdGen(0);
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = configuration.GetConnectionString("Redis"));

// Caching Services
builder.Services.AddCaching(configuration);

// Health Checks
builder.Services.AddHealthChecks(configuration);
builder.Services.AddHealthCheckUI(configuration);

// CORS Configuration
builder.Services.AddCorsConfiguration();

// CQRS
builder.Services.AddCqrs();

// API Documentation
builder.Services.AddSwagger();

// CORS Configuration
builder.Services.AddCorsConfiguration();

var app = builder.Build();

// Configure Pipeline
ConfigurePipeline(app);

app.Run();

// Local Functions
static void ConfigureSignalR(Microsoft.AspNetCore.SignalR.HubOptions options)
{
    options.MaximumReceiveMessageSize = 102400000;
    options.EnableDetailedErrors = true;
}

static void ConfigurePipeline(WebApplication app)
{
    // Database Initialization
    app.IntializeDatabase();

    // Security Middleware
    app.UseMiddleware<DomainWhitelistMiddleware>();
    app.UseCustomExceptionHandler();
    app.UseHsts(app.Environment);
    app.UseHttpsRedirection();

    // Static Files with Caching
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ConfigureStaticFilesCaching
    });

    // API Documentation
    app.UseSwaggerAndUI();

    // Routing
    app.UseRouting();

    // CORS
    app.UseCors(app.Environment.IsDevelopment() ? "DevCors" : "ProdCors");

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Endpoints
    app.MapHub<NotificationHub>("/hub/notifications")
       .RequireCors(app.Environment.IsDevelopment() ? "DevCors" : "ProdCors");

    // Health Check Endpoints
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });

    app.MapHealthChecks("/health/detailed", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                totalDuration = report.TotalDuration.TotalMilliseconds,
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    duration = e.Value.Duration.TotalMilliseconds,
                    exception = e.Value.Exception?.Message,
                    data = e.Value.Data,
                    description = e.Value.Description,
                    tags = e.Value.Tags
                })
            }, new JsonSerializerOptions { WriteIndented = true });

            await context.Response.WriteAsync(result);
        }
    });

    app.UseHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.UseHealthChecks("/health/detailed", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.UseHealthChecksUI(config =>
    {
        config.UIPath = "/health-ui";
    });
/*
    // Health Check UI
    app.MapHealthChecksUI(options =>
    {
        options.UIPath = "/health-ui";
        options.ApiPath = "/health-ui-api";
        options.UseRelativeApiPath = false;
        options.UseRelativeResourcesPath = false;
    });*/

    app.MapGet("/", () => Results.Redirect("/swagger/index.html"));
    app.MapControllers();
}

static void ConfigureStaticFilesCaching(StaticFileResponseContext context)
{
    var path = context.File?.PhysicalPath;
    if (path != null && IsImageFile(path))
    {
        var maxAge = TimeSpan.FromDays(365);
        context.Context.Response.Headers.Append("Cache-Control",
            $"max-age={maxAge.TotalSeconds:0}");
    }
}

static bool IsImageFile(string path)
{
    var imageExtensions = new[] { ".gif", ".jpg", ".jpeg", ".png", ".svg", ".webp" };
    return imageExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
}