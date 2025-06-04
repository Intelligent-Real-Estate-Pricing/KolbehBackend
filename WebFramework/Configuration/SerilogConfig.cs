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

            var logFolder = Path.Combine(AppContext.BaseDirectory, "Logs");
            Directory.CreateDirectory(logFolder);

            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var logFilePath = Path.Combine(logFolder, $"{date}.log");

            // حذف فایل قبلی اگر وجود دارد
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

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
        };
}
