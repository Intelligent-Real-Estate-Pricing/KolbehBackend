using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Application.Cqrs.Commands.Dispatcher;
using Application.Cqrs.Commands;
using Application.Cqrs.Queris.Dispatcher;
using Application.Cqrs.Queris;
using Data.Contracts;
using Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Cqrs.Behaviors;
using FluentValidation;

public static class ServiceExtensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblies(typeof(ICommand<>).Assembly);
            // Register sequence is important
            c.AddOpenBehavior(typeof(ValidationBehavior<,>));
            c.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));
            c.AddOpenBehavior(typeof(TransactionBehavior<,>));
            c.AddOpenBehavior(typeof(CachingBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ICommand<>).Assembly);
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }

    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetSection("Redis")["ConnectionString"]
           ?? configuration.GetConnectionString("Redis")
           ?? throw new InvalidOperationException("Redis connection string is required");


        try
        {
            // Redis Distributed Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "KolbehApp";
            });

            // Redis Connection Multiplexer
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString = redisConnectionString;
                var configuration = ConfigurationOptions.Parse(connectionString);
                configuration.AbortOnConnectFail = false;
                configuration.ConnectRetry = 3;
                configuration.ConnectTimeout = 5000;

                return ConnectionMultiplexer.Connect(configuration);
            });
        }
        catch (Exception ex)
        {
            // Fallback to in-memory cache if Redis is not available
            services.AddDistributedMemoryCache();
            // Log warning about Redis connection failure
            Console.WriteLine($"Redis connection failed, falling back to in-memory cache: {ex.Message}");
        }

        services.AddMemoryCache();
        return services;
    }

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DevCors", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            options.AddPolicy("ProdCors", builder =>
            {
                builder.WithOrigins(
                    "https://kolbeh.liara.run",
                    "https://localhost:5001",
                    "http://localhost:5000"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var healthChecksBuilder = services.AddHealthChecks();

        // Database Health Check
        var connectionString = configuration.GetConnectionString("Database");
        if (!string.IsNullOrEmpty(connectionString))
        {
            healthChecksBuilder.AddSqlServer(
                connectionString,
                name: "sql-server",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "database", "sql", "sqlserver" },
                timeout: TimeSpan.FromSeconds(30));
        }

        // Redis Health Check
        var redisConnectionString = configuration.GetConnectionString("Redis")
            ?? configuration.GetSection("Redis:ConnectionString").Value;
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            healthChecksBuilder.AddRedis(
                redisConnectionString,
                name: "redis",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "cache", "redis" },
                timeout: TimeSpan.FromSeconds(10));
        }

        // Basic System Health Checks
        healthChecksBuilder.AddCheck("self", () => HealthCheckResult.Healthy(), new[] { "self" });

        return services;
    }

    public static IServiceCollection AddHealthCheckUI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(30); // Check every 30 seconds
            options.MaximumHistoryEntriesPerEndpoint(50);
            options.SetApiMaxActiveRequests(1);

            // Add endpoints to monitor
            options.AddHealthCheckEndpoint("Kolbeh API", "/health");
            options.AddHealthCheckEndpoint("Kolbeh API Detailed", "/health/detailed");

            // Custom webhook notifications (اختیاری)
            var webhookUrl = configuration.GetSection("HealthChecks:WebhookUrl").Value;
            if (!string.IsNullOrEmpty(webhookUrl))
            {
                options.AddWebhookNotification("webhook",
                    uri: webhookUrl,
                    payload: "{\n  \"message\": \"Health check failed: [[LIVENESS]]\",\n  \"description\": \"[[DESCRIPTIONS]]\",\n  \"timestamp\": \"[[TIMESTAMP]]\"\n}",
                    restorePayload: "{\n  \"message\": \"Health check recovered: [[LIVENESS]]\",\n  \"description\": \"[[DESCRIPTIONS]]\",\n  \"timestamp\": \"[[TIMESTAMP]]\"\n}");
            }
        })
        .AddInMemoryStorage(); // استفاده از In-Memory storage

        return services;
    }
}