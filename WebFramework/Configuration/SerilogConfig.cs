using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog;

public static class SerilogConfig
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (hostingContext, loggerConfiguration) =>
        {
            var env = hostingContext.HostingEnvironment;
            var applicationName = env.ApplicationName;
            var environmentName = env.EnvironmentName;
            var logFolder = "/tmp/logs";

            try
            {
                Directory.CreateDirectory(logFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create log directory: {ex.Message}");
            }

            var logFilePath = Path.Combine(logFolder, "log-.txt");
            loggerConfiguration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithProperty("EnvironmentName", environmentName)
                .WriteTo.Console()
                .WriteTo.File(
                    logFilePath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                    shared: true
                );
        };
}