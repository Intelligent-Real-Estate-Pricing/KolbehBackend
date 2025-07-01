using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

public static class SerilogConfig
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (hostingContext, loggerConfiguration) =>
        {
            var env = hostingContext.HostingEnvironment;
            var applicationName = env.ApplicationName;
            var environmentName = env.EnvironmentName;

            // استفاده از /tmp که در تمام سیستم‌های Unix قابل نوشتن است
            var logFolder = "/tmp/logs";

            try
            {
                Directory.CreateDirectory(logFolder);
                Console.WriteLine($"Log directory created successfully at: {logFolder}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create log directory at {logFolder}: {ex.Message}");
                // اگر نتوانست پوشه ایجاد کند، فقط console logging استفاده می‌کند
                loggerConfiguration.MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .Enrich.WithProperty("ApplicationName", applicationName)
                    .Enrich.WithProperty("EnvironmentName", environmentName)
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
                return;
            }

            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var logFilePath = Path.Combine(logFolder, $"{date}.log");

            // حذف فایل قبلی اگر وجود دارد
            try
            {
                if (File.Exists(logFilePath))
                {
                    File.Delete(logFilePath);
                    Console.WriteLine($"Previous log file deleted: {logFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not delete previous log file: {ex.Message}");
            }

            // تنظیم کامل logger
            loggerConfiguration.MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithProperty("EnvironmentName", environmentName)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path: logFilePath,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Infinite,
                    shared: true
                );

            Console.WriteLine($"Logging configured successfully. File path: {logFilePath}");
        };
}